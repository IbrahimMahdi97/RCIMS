namespace Entities.Exceptions;

public class AccountAlreadyExistsBadRequestException (string accountName, int accountType)
    : BadRequestException($"Account: {accountName}, with type {accountType} already exists");