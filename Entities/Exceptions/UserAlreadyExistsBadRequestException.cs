namespace Entities.Exceptions;

public class UserAlreadyExistsBadRequestException(string username)
    : BadRequestException($"Username: {username} already exists");