using Shared.DataTransferObjects;

namespace Service.Interface;

public interface IContractService
{
    Task<int> ApplyForContract(ContractApplyDto contract, int userId);
    Task<int> CreateByAdmin(ContractCreateByAdminDto contract, int userId);
    Task<IEnumerable<ContractDto>> GetAll();
    Task<ContractDto> GetById(string id);
    Task Update(ContractForManipulationDto contract, string id, int userId);
    Task Delete(string id);
}