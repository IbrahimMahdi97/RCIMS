namespace Repository.Query;

public static class RoleQuery
{
    public const string UserRolesByIdQuery =
        @"SELECT R.id, R.name FROM rcims_roles R
                JOIN rcims_userroles UR on R.id = UR.roleid
                WHERE UR.UserId = @id";

    public const string InsertUserRolesQuery =
        @"INSERT INTO rcims_userroles (userid, roleid)
              VALUES(@UserId, @RoleId)";
    public const string DeleteUserRolesQuery =
        @"DELETE FROM rcims_userroles WHERE userid =@Id";

    public const string AllQuery = @"SELECT id, description, name, createdby, createddate, updatedby, updateddate FROM rcims_roles where isactive =True";
    public const string CreateQuery = @"INSERT INTO rcims_roles (name,description,iscanedit,createdby,createddate,updatedby,updateddate) 
                                                   VALUES (@Name,@Description,@UserId,NOW(),@UserId,NOW()) RETURNING id";
    public const string UpdateQuery = @"UPDATE rcims_roles 
                                            SET name=@Name, description=@Description, isactive=@IsActive, updatedby=@UserId, updateddate=NOW()  
                                        WHERE id=@Id";
    public const string GetByIdQuery = @"SELECT * FROM rcims_roles WHERE id=@Id";
    public const string DeleteQuery = @"Update rcims_roles Set isactive=false, updatedby=@UserId, updateddate=NOW()  WHERE id=@Id";
}