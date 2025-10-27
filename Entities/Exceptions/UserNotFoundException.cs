namespace Entities.Exceptions;

public class UserNotFoundException(string id) 
    : NotFoundException($"The user with id: {id} doesn't exist in the database.");