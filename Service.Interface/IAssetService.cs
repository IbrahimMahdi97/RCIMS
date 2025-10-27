using Shared.DataTransferObjects;

namespace Service.Interface;

public interface IAssetService
{
    Task<int> Create(AssetForManipulationDto asset, int userId);
    Task<IEnumerable<AssetDto>> GetAll();
    Task<AssetDto> GetById(string id);
    Task Update(AssetForManipulationDto asset, string id, int userId);
    Task Delete(string id);
}