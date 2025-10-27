namespace Interface;

public interface IRepositoryManager
{
    IUserRepository User { get; }
    IRoleRepository Role { get; }
    IAccountRepository Account { get; }
    IAssetRepository  Asset { get; }
    IContractRepository Contract { get; }
    IInstallmentRepository Installment { get; }
    IPaymentRepository Payment { get; }
    IPlanRepository Plan { get; }
}