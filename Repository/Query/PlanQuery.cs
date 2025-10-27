namespace Repository.Query;

public static class PlanQuery
{
    public const string InsertQuery = @"INSERT INTO rcims_plans
                                            (name, description, calculationtype, months, equation, period, percent, 
                                             maxallow, createdby, createddate, updatedby, updateddate)
                                        VALUES (@Name, @Description, @CalculationType, @Month, @Equation, @Period, @Percent, 
                                                @MaxAllow, @UserId, NOW(), @UserId, NOW())
                                        RETURNING id";
    public const string GetAllQuery = @"SELECT * FROM rcims_plans WHERE isactive=true";
    public const string GetByIdQuery = @"SELECT * FROM rcims_plans WHERE uuid=@Id";
    public const string UpdateQuery = @"UPDATE rcims_plans 
                                            SET name=@Name, description=@Description, 
                                                calculationtype=@CalculationType,  months=@Months, 
                                                equation=@Equation, period=@Period, percent=@Percent,
                                                maxallow=@MaxAllow, updatedby=@UserId, updateddate=NOW() WHERE uuid=@Id";
    public const string DeleteQuery = @"UPDATE rcims_plans SET isactive=false WHERE uuid=@Id";
    public const string GetByNameQuery = @"SELECT * FROM rcims_plans WHERE name=@Name AND isactive=true";
    public const string GetIdByUUIDQuery = @"SELECT id FROM rcims_plans WHERE uuid=@Id";
}