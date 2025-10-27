namespace Repository.Query;

public static class UserQuery
{
    public const string UserIdByUUIDQuery =
        @"SELECT id FROM rcims_users WHERE uuid=@UUID";
    public const string UserIdByUsernameQuery =
        @"SELECT id FROM rcims_users WHERE username=@Username AND isactive = true";
    public const string UserByCredentialsUsernameQuery =
        @"SELECT 
            *
        FROM rcims_users 
        WHERE username=@Username AND password = @Password  AND isactive = true";
    public const string UserRolesByUserIdQuery =
        @"SELECT id, name, UR.userid FROM rcims_roles R
                       JOIN rcims_userroles UR on R.id = UR.roleid
                       WHERE UR.userid = @Id";
    public const string UpdateRefreshTokenByIdQuery =
        @"UPDATE rcims_users SET refreshtoken = @refreshToken, refreshtokenexpirytime = @refreshTokenExpiryTime WHERE id = @id";
    public const string CreateUserQuery =
        @"INSERT INTO rcims_users (firstname, lastname, gender, birthdate, email, phone,
                            username, createdby, createddate, updatedby, updateddate)
         VALUES(@FirstName, @LastName, @Gender, @Birthdate, @Email, @Phone, 
                        @UserName, @UserId, NOW(), @UserId, NOW()) RETURNING id";
    public const string AddEncryptedPasswordByIdQuery =
        @"UPDATE rcims_users SET password = @Password WHERE id = @Id";
    public const string GetByIdQuery = @"SELECT * 
                                        from rcims_users
                                        WHERE id=@Id";
    public const string UpdateUserQuery =
        @"UPDATE rcims_users SET firstname=@FirstName, lastname=@LastName, gender=@Gender, birthdate=@Birthdate, email=@Email, 
                            phone=@Phone, username=@UserName, updatedby=@UpdatedBy, updateddate=NOW()
            WHERE id=@UserId RETURNING id";
    public const string AllUsersQuery = @"SELECT 
                us.id,
                us.uuid,
                CONCAT_WS(' ', us.firstname, us.lastname) FullName,
                us.gender,
                us.birthdate, 
                us.email,
                us.phone,
                us.username,
                us.createddate,
                us.updateddate,
                us.isactive,
                rol.id as roleid,
                rol.name as rolename,
                rol.uuid as roleuuid,
                rol.description as roledescription
            FROM rcims_users us
            INNER JOIN rcims_userroles ur ON (us.id = ur.userid)
            INNER JOIN rcims_roles rol ON (rol.id = ur.roleid)
            WHERE (@FullName IS NULL OR CONCAT_WS(' ', us.firstname, us.lastname) ILIKE '%' || @FullName || '%')
                AND (@Phone IS NULL OR us.phone ILIKE '%' || @Phone || '%' )
                AND (@Email IS NULL OR us.email ILIKE '%' || @Email || '%')
                AND (@UserName IS NULL OR us.username ILIKE '%' || @UserName || '%')
                AND (@RoleId=0 OR rol.id=@RoleId)
            ORDER BY us.id DESC
            LIMIT @PageSize OFFSET @Offset";

    public const string DeleteQuery = @"UPDATE rcims_users SET isactive=false, updatedby=@UserId, updateddate=NOW() 
                                                WHERE id=@id";
    
}