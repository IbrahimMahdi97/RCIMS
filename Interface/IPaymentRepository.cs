using Shared.DataTransferObjects;

namespace Interface;

public interface IPaymentRepository
{
    Task<int> Create(PaymentForManipulationDto payment, int userId);
    Task<IEnumerable<PaymentDto>> GetAll();
    Task<PaymentDto> GetById(string id);
    Task Update(PaymentForManipulationDto payment, string id, int userId);
    Task Delete(string id);
}