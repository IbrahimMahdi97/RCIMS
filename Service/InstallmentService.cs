using Interface;
using Service.Interface;

namespace Service;

internal sealed class InstallmentService : IInstallmentService
{
    private readonly IRepositoryManager _repository;

    public InstallmentService(IRepositoryManager repository)
    {
        _repository = repository;
    }
}