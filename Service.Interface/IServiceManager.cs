namespace Service.Interface;

public interface IServiceManager
{
    IUserService UserService { get; }
    IRoleService RoleService { get; }
    IAccountService AccountService { get; }
    IPlanService PlanService { get; }
    IAssetService AssetService { get; }
    IContractService ContractService { get; }
    IInstallmentService InstallmentService { get; }
    IPaymentService PaymentService { get; }
}