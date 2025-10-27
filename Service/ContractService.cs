using Entities.Exceptions;
using Interface;
using Service.Interface;
using Shared.DataTransferObjects;

namespace Service;

internal sealed class ContractService : IContractService
{
    private readonly IRepositoryManager _repository;

    public ContractService(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<int> ApplyForContract(ContractApplyDto contract, int userId)
    {
        var asset = await _repository.Asset.GetById(contract.AssetId) ?? throw new AssetNotFoundException();
        var plan = await _repository.Plan.GetById(contract.PlanId) ?? throw new PlanNotFoundException();
        var assetId = await _repository.Asset.GetIdByUUID(contract.AssetId);
        var planId = await _repository.Plan.GetIdByUUID(contract.PlanId);
        var contractNumber = await GetLastContractNumber() + 1;
        var contractCreate = new ContractForManipulationDto
        {
            ContractNumber = contractNumber.ToString(),
            ContractDate = DateTime.Now,
            Asset = assetId,
            Plan = planId,
            ContractStatus = 2,
            ContractFor = userId,
            TotalAmount = asset.ListPrice,
            GraceDays = plan.MaxAllow,
            Description = contract.Description
        };
        var result = await Create(contractCreate, userId);
        return result;
    }

    public async Task<int> CreateByAdmin(ContractCreateByAdminDto contract, int userId)
    {
        var asset = await _repository.Asset.GetById(contract.AssetId) ?? throw new AssetNotFoundException();
        var plan = await _repository.Plan.GetById(contract.PlanId) ?? throw new PlanNotFoundException();
        var assetId = await _repository.Asset.GetIdByUUID(contract.AssetId);
        var planId = await _repository.Plan.GetIdByUUID(contract.PlanId);
        var customerId = await _repository.User.GetIdByUUID(contract.UserId);
        if (customerId <= 0) throw new UserNotFoundException(contract.UserId);
        var contractNumber = await GetLastContractNumber() + 1;
        int price = asset.ListPrice;
        int subtotal = price - contract.DownPayment;
        int discountamount = 0;
        decimal discount = 0;
        if (contract.Discount > 0)
        {
            discount = contract.Discount;
            discountamount = Convert.ToInt32((price - contract.DownPayment) * discount);
            subtotal = price - discountamount;
        }

        var contractCreate = new ContractForManipulationDto
        {
            ContractNumber = contractNumber.ToString(),
            ContractDate = DateTime.Now,
            Asset = assetId,
            Plan = planId,
            ContractStatus = 2,
            ContractFor = customerId,
            TotalAmount = price,
            DownPayment = contract.DownPayment,
            PaidAmount = contract.DownPayment,
            Subtotal = subtotal,
            GraceDays = plan.MaxAllow,
            Discount = discount,
            DiscountAmount = discountamount,
            Description = contract.Description
        };
        var result = await Create(contractCreate, userId);
        return result;
    }

    private async Task<int> Create(ContractForManipulationDto contract, int userId)
    {
        var result = await _repository.Contract.Create(contract, userId);
        return result;
    }

    public async Task<IEnumerable<ContractDto>> GetAll()
    {
        var result = await _repository.Contract.GetAll();
        return result;
    }

    public async Task<ContractDto> GetById(string id)
    {
        var result = await _repository.Contract.GetById(id) ?? throw new ContractNotFoundException();
        return result;
    }

    public async Task Update(ContractForManipulationDto contract, string id, int userId)
    {
        await GetById(id);
        await _repository.Contract.Update(contract, id, userId);
    }

    public async Task Delete(string id)
    {
        await GetById(id);
        await _repository.Contract.Delete(id);
    }

    private async Task<int> GetLastContractNumber()
    {
        var result = await _repository.Contract.GetLastContractNumber();
        if (!int.TryParse(result, out var id))
            id = 0;
        return id;
    }
}