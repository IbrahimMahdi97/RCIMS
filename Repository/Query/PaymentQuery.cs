namespace Repository.Query;

public static class PaymentQuery
{
    public const string InsertQuery = @"INSERT INTO rcims_payments
                                                (documentnumber, documentdate, paymentmethod, paymenttype, contractid, 
                                                 installmentid, amount, currency, documentstatus, documentfor, accountid,
                                                 description, comments, createdby, createddate, updatedby, updateddate)
                                        VALUES (@DocumentNumber, @DocumentDate, @PaymentMethod, @PaymentType, @ContractId,
                                                @InstallmentId, @Amount, @Currency, @DocumentStatus, @DocumentFor, @AccountId,
                                                @Description, @Comments, @UserId, NOW(), @UserId, NOW())
                                        RETURNING id";
    public const string GetAllQuery = @"SELECT * FROM rcims_payments WHERE isactive=true";
    public const string GetByIdQuery = @"SELECT * FROM rcims_payments WHERE uuid=@Id";
    public const string UpdateQuery = @"UPDATE rcims_payments 
                                            SET documentnumber=@DocumentNumber, documentdate=@DocumentDate, 
                                                paymentmethod=@PaymentMethod, paymenttype=@PaymentType,
                                                contractid=@ContractId,  installmentid=@InstallmentId, amount=@Amount,
                                                currency=@Currency, documentstatus=@DocumentStatus, 
                                                documentfor=@DocumentFor, account=@Account, description=@Description, 
                                                updatedby=@UserId, updateddate=NOW() 
                                            WHERE uuid=@Id";
    public const string DeleteQuery = @"UPDATE rcims_payments SET isactive=false WHERE uuid=@Id";
}