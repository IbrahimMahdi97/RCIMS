using Shared.DataTransferObjects;

namespace Interface;

public interface IInstallmentRepository
{
    Task<int> Create(InstallmentForManipulationDto installment, int userId);
    Task<IEnumerable<InstallmentDto>> GetAll();
    Task<InstallmentDto> GetById(string id);
    Task Update(InstallmentForManipulationDto installment, string id, int userId);
    Task Delete(string id);
}