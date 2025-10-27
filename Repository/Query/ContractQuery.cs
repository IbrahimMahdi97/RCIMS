namespace Repository.Query;

public static class ContractQuery
{
    public const string InsertQuery = @"INSERT INTO rcims_contracts(contractnumber, contractdate, contractstatus, 
                                        contractfor, asset, plan, totalamount, downpayment, gracedays, 
                                        subtotal, createdby, createddate, updatedby, updateddate)
                                        VALUES (@ContractNumber, @ContractDate, @ContractStatus, 
                                                @ContractFor, @Asset, @Plan, @TotalAmount, @DownPayment, @GraceDays, 
                                                @TotalAmount, @UserId, NOW(), @UserId, NOW())
                                        RETURNING id";

    private const string SelectQuery = @"SELECT 
                                        contract.uuid AS UUID,
                                        contract.contractnumber AS ContractNumber,
                                        contract.contractdate AS ContractDate,
                                        CASE contract.contractstatus
                                          WHEN 1 THEN 'Draft'
                                          WHEN 2 THEN 'Submitted' 
                                          WHEN 3 THEN 'NeedUpdate'
                                          WHEN 4 THEN 'Approved'
                                          WHEN 5 THEN 'Cancelled'
                                          WHEN 6 THEN 'Suspended'
                                          WHEN 7 THEN 'Closed'
                                            ELSE ''
                                        END   AS ContractStatusName,
                                        contract.description AS Description,
                                        contract.totalamount AS TotalAmount,
                                        contract.downpayment AS DownPayment,
                                        contract.paidamount AS PaidAmount,
                                        contract.discount AS Discount,
                                        contract.discountamount AS DiscountAmount,
                                        contract.gracedays AS GraceDays,
                                        contract.dailypenaltyrate AS DailyPenaltyRate,
                                        contract.earlypayoffdiscountpercent AS EarlyPayOffDiscountPercent,
                                        contract.subtotal AS Subtotal,
                                        CONCAT_WS(' ', customer.firstname, customer.lastname) CustomerName,
                                        as.name AS AssetName,
                                        as.area AS Area,
                                        CASE as.assettype 
                                            WHEN 1 THEN 'House'
                                            WHEN 2 THEN 'Appartement'
                                           ELSE ''
                                        END AS AssetType,
                                        plan.name AS PlanName
                                        FROM rcims_contracts contract 
                                        INNER JOIN rcims_assets as ON contract.asset = as.id
                                        INNER JOIN rcims_plans plan ON contract.plan = plan.id
                                        WHERE ";
    public const string GetAllQuery = SelectQuery + "contract.isactive=true";
    public const string GetByIdQuery = SelectQuery + "contract.uuid=@Id";
    public const string UpdateQuery = @"UPDATE rcims_contracts 
                                            SET contractnumber=@ContractNumber, contractdate=@ContractDate, 
                                                contractstatus=@ContractStatus,  description=@Description, 
                                                contractfor=@ContractFor, asset=@Asset, plan=@Plan,
                                                totalamount=@TotalAmount, paidamount=@PaidAmount,  discount=@Discount, 
                                                discountamount=@DiscountAmount,  graceDays=@GraceDays, 
                                                dailypenaltyrate=@DailyPenaltyRate, 
                                                earlypayoffdiscountpercent=@EarlyPayoffDiscountPercent, 
                                                subtotal=@Subtotal, updatedby=@UserId, updateddate=NOW() WHERE uuid=@Id";
    public const string DeleteQuery = @"UPDATE rcims_contracts SET isactive=false WHERE uuid=@Id";
    public const string GetLastContractNumberQuery = @"SELECT MAX(contractnumber) FROM rcims_contracts WHERE isactive=true";
}