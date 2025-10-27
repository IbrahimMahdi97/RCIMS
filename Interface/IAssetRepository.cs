using Shared.DataTransferObjects;

namespace Interface;

public interface IAssetRepository
{
    Task<int> Create(AssetForManipulationDto asset, int userId);
    Task<IEnumerable<AssetDto>> GetAll();
    Task<AssetDto> GetById(string id);
    Task Update(AssetForManipulationDto asset, string id, int userId);
    Task Delete(string id);
    Task<int> GetIdByUUID(string uuid);
}