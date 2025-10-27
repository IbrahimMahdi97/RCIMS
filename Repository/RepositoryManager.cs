using Interface;

namespace Repository;

public class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<IUserRepository> _userRepository;
    private readonly Lazy<IRoleRepository> _roleRepository;
    private readonly Lazy<IAccountRepository> _accountRepository;
    private readonly Lazy<IAssetRepository> _assetRepository;
    private readonly Lazy<IContractRepository>  _contractRepository;
    private readonly Lazy<IInstallmentRepository> _installmentRepository;
    private readonly Lazy<IPaymentRepository> _paymentRepository;
    private readonly Lazy<IPlanRepository> _planRepository;

    public RepositoryManager(DapperContext dapperContext)
    {
        _userRepository = new Lazy<IUserRepository>(() => new UserRepository(dapperContext));
        _roleRepository = new Lazy<IRoleRepository>(() => new RoleRepository(dapperContext));
        _accountRepository = new Lazy<IAccountRepository>(() => new AccountRepository(dapperContext));
        _assetRepository = new Lazy<IAssetRepository>(() => new AssetRepository(dapperContext));
        _contractRepository = new Lazy<IContractRepository>(() => new ContractRepository(dapperContext));
        _installmentRepository = new Lazy<IInstallmentRepository>(() => new InstallmentRepository(dapperContext));
        _paymentRepository = new Lazy<IPaymentRepository>(() => new PaymentRepository(dapperContext));
        _planRepository = new Lazy<IPlanRepository>(() => new PlanRepository(dapperContext));
    }
    
    public IUserRepository User => _userRepository.Value;
    public IRoleRepository Role => _roleRepository.Value;
    public IAccountRepository Account => _accountRepository.Value;
    public IAssetRepository Asset => _assetRepository.Value;
    public IContractRepository Contract => _contractRepository.Value;
    public IInstallmentRepository Installment => _installmentRepository.Value;
    public IPaymentRepository Payment => _paymentRepository.Value;
    public IPlanRepository Plan => _planRepository.Value;
}