using Entities.Exceptions;
using Interface;
using Service.Interface;
using Shared.DataTransferObjects;

namespace Service;

internal sealed class AssetService : IAssetService
{
    private readonly IRepositoryManager _repository;

    public AssetService(IRepositoryManager repository)
    {
        _repository = repository;
    }
    
    public async Task<int> Create(AssetForManipulationDto asset, int userId)
    {
        var result = await _repository.Asset.Create(asset, userId);
        return result;
    }

    public async Task<IEnumerable<AssetDto>> GetAll()
    {
        var result = await _repository.Asset.GetAll();
        return result;
    }

    public async Task<AssetDto> GetById(string id)
    {
        var result = await _repository.Asset.GetById(id) ?? throw new AssetNotFoundException();
        return result;
    }

    public async Task Update(AssetForManipulationDto asset, string id, int userId)
    {
        var oldAccount = await GetById(id);
        await _repository.Asset.Update(asset, id, userId);
    }

    public async Task Delete(string id)
    {
        var account = await GetById(id);
        await _repository.Asset.Delete(id);
    }
}