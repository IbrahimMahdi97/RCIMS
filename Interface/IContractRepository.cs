using Shared.DataTransferObjects;

namespace Interface;

public interface IContractRepository
{
    Task<int> Create(ContractForManipulationDto contract, int userId);
    Task<IEnumerable<ContractDto>> GetAll();
    Task<ContractDto> GetById(string id);
    Task Update(ContractForManipulationDto asset, string id, int userId);
    Task Delete(string id);
    Task<string> GetLastContractNumber();
}