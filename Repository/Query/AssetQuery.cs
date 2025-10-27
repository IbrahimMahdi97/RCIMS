namespace Repository.Query;

public static class AssetQuery
{
    public const string InsertQuery = @"INSERT INTO rcims_assets(name, description, assettype, area, price, listprice, status, createdby, createddate, updatedby, updateddate)
                                        VALUES (@Name, @Description, @AssetType, @Area, @Price, @ListPrice, @Status, @UserId, NOW(), @UserId, NOW())
                                        RETURNING id";
    public const string GetAllQuery = @"SELECT * FROM rcims_assets WHERE isactive=true";
    public const string GetByIdQuery = @"SELECT * FROM rcims_assets WHERE uuid=@Id";
    public const string UpdateQuery = @"UPDATE rcims_assets 
                                            SET name=@Name, description=@Description, 
                                                assettype=@AssetType,  area=@Area, 
                                                price=@Price, listprice=@ListPrice, status=@Status,
                                                updatedby=@UserId, updateddate=NOW() WHERE uuid=@Id";
    public const string DeleteQuery = @"UPDATE rcims_assets SET isactive=false WHERE uuid=@Id";
    public const string GetIdByUUIDQuery = @"SELECT id FROM rcims_assets WHERE uuid=@Id";
}