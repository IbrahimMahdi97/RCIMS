using Interface;
using Microsoft.Extensions.Configuration;
using Service.Interface;

namespace Service;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IUserService> _userService;
    private readonly Lazy<IRoleService> _roleService;
    private readonly Lazy<IAccountService> _accountService;
    private readonly Lazy<IPlanService> _planService;
    private readonly Lazy<IAssetService> _assetService;
    private readonly Lazy<IContractService> _contractService;
    private readonly Lazy<IInstallmentService> _installmentService;
    private readonly Lazy<IPaymentService> _paymentService;

    public ServiceManager(IRepositoryManager repositoryManager, IConfiguration configuration)
    {
        Lazy<IFileStorageService> fileStorageService = new(() => new FileStorageService());
        Lazy<IFirebaseService> firebaseService = new(() => new FirebaseService());
        _userService = new Lazy<IUserService>(() =>
            new UserService(repositoryManager, fileStorageService.Value, configuration));
        _roleService = new Lazy<IRoleService>(() => new RoleService(repositoryManager));
        _accountService = new Lazy<IAccountService>(() => new AccountService(repositoryManager));
        _planService = new Lazy<IPlanService>(() => new PlanService(repositoryManager));
        _assetService = new Lazy<IAssetService>(() => new AssetService(repositoryManager));
        _contractService = new Lazy<IContractService>(() => new ContractService(repositoryManager));
        _installmentService = new Lazy<IInstallmentService>(() => new InstallmentService(repositoryManager));
        _paymentService = new Lazy<IPaymentService>(() => new PaymentService(repositoryManager));
    }
    
    public IUserService UserService => _userService.Value;
    public IRoleService RoleService => _roleService.Value;
    public IAccountService AccountService => _accountService.Value;
    public IPlanService PlanService => _planService.Value;
    public IAssetService AssetService => _assetService.Value;
    public IContractService ContractService => _contractService.Value;
    public IInstallmentService InstallmentService => _installmentService.Value;
    public IPaymentService PaymentService => _paymentService.Value;
}