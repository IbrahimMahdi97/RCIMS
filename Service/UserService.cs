using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Entities.Exceptions;
using Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.Interface;
using Shared.DataTransferObjects;
using Shared.Helpers;
using Shared.Parameters;

namespace Service;

internal sealed class UserService : IUserService
{
    private readonly IRepositoryManager _repository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IConfiguration _configuration;

    public UserService(IRepositoryManager repository, IFileStorageService fileStorageService,
        IConfiguration configuration)
    {
        _repository = repository;
        _fileStorageService = fileStorageService;
        _configuration = configuration;
    }
    public async Task<UserDto> ValidateUser(UserForAuthenticationDto userForAuth)
    {
        var user = await CheckIfUserExists(userForAuth);
        var userId = await _repository.User.GetIdByUUID(user.UUID);
        var tokens = await CreateToken(user, userId, true);
        user.AccessToken = tokens.AccessToken;
        user.RefreshToken = tokens.RefreshToken;
        user.Roles = await _repository.User.GetUserRoles(userId);
        return user;
    }
    private async Task<UserDto> CheckIfUserExists(UserForAuthenticationDto dto)
    {
        var id = await _repository.User.FindIdByUsername(dto.Username);
        if (id == 0) throw new UsernameNotFoundException(dto.Username);

        var user = await _repository.User.FindByCredentialsUsername(dto.Username,
            (dto.Password + id).ToSha512());
        if (user is null) throw new InvalidPasswordForUserUnauthorizedException(dto.Username);

        var tokens = await CreateToken(user, id, true);
        user.AccessToken = tokens.AccessToken;
        user.RefreshToken = tokens.RefreshToken;
        user.Roles = await _repository.User.GetUserRoles(id);
        return user;
    }
    private async Task<TokenDto> CreateToken(UserDto user, int userId, bool populateExp)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims(user, userId);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        var refreshToken = user.RefreshToken = GenerateRefreshToken();
        var refreshTokenExpiryTime = populateExp ? DateTime.Now.AddDays(7) : user.RefreshTokenExpiryTime;
        await _repository.User.UpdateRefreshToken(userId, refreshToken, refreshTokenExpiryTime);
        return new TokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
    private SigningCredentials GetSigningCredentials()
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = Encoding.UTF8.GetBytes(jwtSettings["secretKey"]!);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }
    private async Task<List<Claim>> GetClaims(UserDto user, int userId)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.MobilePhone, user.Phone!),

            new(ClaimTypes.NameIdentifier, userId.ToString())
        };

        var roles = await _repository.Role.GetUserRoles(userId);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role.Name!)));

        return claims;
    }
    private JwtSecurityToken GenerateTokenOptions
        (SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var tokenOptions = new JwtSecurityToken(
            issuer: jwtSettings["validIssuer"],
            audience: jwtSettings["validAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expiryInMinutes"])),
            signingCredentials: signingCredentials
        );

        return tokenOptions;
    }
    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    public async Task<int> CreateUser(UserForCreationDto userForCreationDto, int creatorId)
    {
        var userIdExist = await _repository.User.FindIdByUsername(userForCreationDto.UserName);
        if (userIdExist > 0)
            throw new UserAlreadyExistsBadRequestException(userForCreationDto.UserName);
        var result = await _repository.User.CreateUser(userForCreationDto, creatorId);
        if (result <= 0)
            throw new UserAlreadyExistsBadRequestException(userForCreationDto.UserName);
        await _repository.User.AddUserRoles(userForCreationDto.Role, result);
        return result;
    }
    public async Task<UserDto> GetById(string id)
    {
        var userId = await _repository.User.GetIdByUUID(id);
        if (userId <= 0) throw new UserNotFoundException(id);
        var user = await _repository.User.GetById(userId) ;
        user.Roles = await _repository.User.GetUserRoles(userId);
        return user;
    }
    public async Task<UserDto> GetMyDetails(int id)
    {
        var user = await _repository.User.GetById(id);
        user.Roles = await _repository.User.GetUserRoles(id);
        return user;
    }
    public async Task UpdatePassword(UpdateUserPasswordDto updateUserPasswordDto)
    {
        var userId = await _repository.User.GetIdByUUID(updateUserPasswordDto.Id);
        var user = GetById(updateUserPasswordDto.Id);
        var hashedPassword = (updateUserPasswordDto.Password + updateUserPasswordDto.Id).ToSha512();
        await _repository.User.UpdatePassword(userId, hashedPassword);
    }
    public async Task UpdateUser(UserForCreationDto userForCreationDto, string id, int updatedBy)
    {
        var user = await GetById(id);
        var userId = await _repository.User.GetIdByUUID(id);
        userForCreationDto.Password = (userForCreationDto.Password + id).ToSha512();
        var result = await _repository.User.UpdateUser(userForCreationDto, userId, updatedBy);
        if (result <= 0)
            throw new UserAlreadyExistsBadRequestException(userForCreationDto.UserName);
        var currentRoles = user.Roles.Select(u => userId).ToList();
        var rolesToDelete = currentRoles.Except(new List<int> { userForCreationDto.Role }).ToList();
        if (rolesToDelete.Count > 0)
        {
            await _repository.User.DeleteUserRoles(userId);
            await _repository.User.AddUserRoles(userForCreationDto.Role, userId);
        }
    }
    public async Task<PagedList<UserForAllDto>> GetAll(UsersAllParameters parameters)
    { 
        var users = await _repository.User.GetAllUsers(parameters);
        var totalCount = users.Count();
        var pagedUsers = users
        .Skip((parameters.PageNumber - 1) * parameters.PageSize)
        .Take(parameters.PageSize)
        .ToList();
        return new PagedList<UserForAllDto>(users.ToList(), totalCount, parameters.PageNumber, parameters.PageSize);
    }

    public async Task Delete(string id, int userId)
    {
        var user = await GetById(id) ?? throw new UserNotFoundException(id);
        var intId = await _repository.User.GetIdByUUID(id);
        await _repository.User.Delete(intId, userId);
    }
}