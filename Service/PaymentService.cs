using Interface;
using Service.Interface;

namespace Service;

internal sealed class PaymentService : IPaymentService
{
    private readonly IRepositoryManager _repository;

    public PaymentService(IRepositoryManager repository)
    {
        _repository = repository;
    }
}