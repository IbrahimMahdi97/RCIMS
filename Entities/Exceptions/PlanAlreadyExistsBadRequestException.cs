namespace Entities.Exceptions;

public class PlanAlreadyExistsBadRequestException(string name) 
    : BadRequestException($"Plane with name {name} already exist.");