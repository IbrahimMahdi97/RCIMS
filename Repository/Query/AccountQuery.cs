namespace Repository.Query;

public static class AccountQuery
{
    public const string InsertQuery = @"INSERT INTO rcims_accounts(name, accounttype, openbalance, description, comments, 
                                                                    createdby, createddate, updatedby, updateddate)
                                              VALUES(@Name, @AccountType, @OpenBalance, @Description, @Comments, 
                                                     @UserId, NOW(), @UserId, NOW()) 
                                              RETURNING id";
    public const string GetAllQuery = @"SELECT * FROM rcims_accounts WHERE isactive=true";
    public const string GetByIdQuery = @"SELECT * FROM rcims_accounts WHERE uuid=@Id";

    public const string UpdateQuery = @"UPDATE rcims_accounts 
                                        SET name=@Name, openbalance=@OpenBalance,  description=@Description,  
                                            comments=@Comments, updatedby=@UserId, updateddate=NOW()
                                        WHERE uuid=@Id";

    public const string UpdateUnmatchedBalanceQuery = @"UPDATE rcims_accounts 
                                                        SET unmatchedbalance=unmatchedbalance+@Amount, 
                                                                updatedby=@UserId, updateddate=NOW()
                                                        WHERE uuid=@Id";
    public const string DeleteQuery = @"UPDATE rcims_accounts SET isactive=false, updatedby=@UserId, updateddate=NOW() WHERE uuid=@Id";
    public const string GetByNameTypeQuery = @"SELECT * FROM rcims_accounts 
                                                WHERE name=@Name AND accounttyp=@Type AND isactive=true";
}