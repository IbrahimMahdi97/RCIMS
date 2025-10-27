namespace Entities.Exceptions;

public class InvalidPasswordForUserUnauthorizedException(string username)
    : UnauthorizedException($"Wrong password for the user : {username}.");