namespace Repository.Query;

public static class InstallmentQuery
{
    public const string InsertQuery = @"INSERT INTO rcims_installments
                                                (contract, duedate, status, amount, discount, discountamount,
                                                 penaltyaccrued, subtotal, paidamount, description, 
                                                 createdby, createddate, updatedby, updateddate)
                                        VALUES (@Contract, @DueDate, @Status, @Amount, @Discount, @DiscountAmount,
                                                @PenaltyAccrued, @Subtotal, @PaidAmount, @Description, 
                                                @UserId, NOW(), @UserId, NOW())
                                        RETURNING id";
    public const string GetAllQuery = @"SELECT * FROM rcims_installments WHERE isactive=true";
    public const string GetByIdQuery = @"SELECT * FROM rcims_installments WHERE uuid=@Id";
    public const string UpdateQuery = @"UPDATE rcims_installments 
                                            SET contract=@Contract, duedate=@DueDate, status=@Status, 
                                                amount=@Amount, discount=@Discount, discountamount=@DiscountAmount,
                                                penaltyaccrued=@PenaltyAccrued, subtotal=@Subtotal, paidamount=@PaidAmount,
                                                description=@Description, updatedby=@UserId, updateddate=NOW() 
                                            WHERE uuid=@Id";
    public const string DeleteQuery = @"UPDATE rcims_installments SET isactive=false WHERE uuid=@Id";
}