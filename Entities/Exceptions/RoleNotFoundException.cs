namespace Entities.Exceptions;

public class RoleNotFoundException(int id) 
    : NotFoundException($"The role with id: {id} doesn't exist in the database.");