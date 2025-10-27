namespace Entities.Exceptions;

public class UsernameNotFoundException(string username) 
    : NotFoundException($"username: {username} doesn't exist.");