using FluentMigrator;

namespace RCIMS.Migrations;

[Migration(202510251930)]
public class InitialSeed202510251930 : Migration
{
    public override void Down()
    {
        Delete.FromTable("rcims_userroles").Row(new
        {
            userid = 1,
            roleid = 1,
        });
        Delete.FromTable("rcims_roles").Row(new
        {
            name = "Admin"
        }).Row(new
        {
            name = "User"
        });
        Delete.FromTable("rcims_users").Row(new
        {
            firstname = "super",
            lastname = "admin",
            username = "admin",
            gender = 0,
            birthdate = new DateTime(1997, 1, 11),
            email = "ibrahimmahdi115@gmail.com",
            phone = "07713497153",
            password = "E449F6BDFC2AC9075C2F7903B5F80B60FB8638F0A57AC6E39FD71B2C93FA45141E6DD6830CC141C5982B1FBF84DC91C012A5D241AC850C1FC3571D346597C814"
        });
    }

    public override void Up()
    {
        Insert.IntoTable("rcims_users").Row(new
        {
            firstname = "super",
            lastname = "admin",
            username = "admin",
            gender = 0,
            birthdate = new DateTime(1997, 1, 11),
            email = "ibrahimmahdi115@gmail.com",
            phone = "07713497153",
            password = "E449F6BDFC2AC9075C2F7903B5F80B60FB8638F0A57AC6E39FD71B2C93FA45141E6DD6830CC141C5982B1FBF84DC91C012A5D241AC850C1FC3571D346597C814"
        });
        
        Insert.IntoTable("rcims_roles").Row(new
        {
            name = "Admin"
        }).Row(new
        {
            name = "User"
        });
        
        Insert.IntoTable("rcims_userroles").Row(new
        {
            userid = 1,
            roleid = 1,
        });
    }
}