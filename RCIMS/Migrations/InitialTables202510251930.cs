using FluentMigrator;

namespace RCIMS.Migrations;

[Migration(202510251930)]
public class InitialTables202510251930 : Migration
{
    public override void Down()
    {
        Delete.Table("rcims_payments");
        Delete.Table("rcims_installments");
        Delete.Table("rcims_contracts");
        Delete.Table("rcims_assets");
        Delete.Table("rcims_plans");
        Delete.Table("rcims_accounts");
        Delete.Table("rcims_userroles");
        Delete.Table("rcims_roles");
        Delete.Table("rcims_users");
    }

    public override void Up()
    {
        Create.Table("rcims_users")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("uuid").AsString(50).Unique()
            .WithColumn("firstname").AsString(100).NotNullable()
            .WithColumn("lastname").AsString(100).NotNullable()
            .WithColumn("phone").AsString(20).Nullable()
            .WithColumn("email").AsString(100).Nullable()
            .WithColumn("gender").AsInt32().NotNullable()
            .WithColumn("birthdate").AsDateTime().NotNullable()
            .WithColumn("username").AsString(100).NotNullable()
            .WithColumn("password").AsString(100).NotNullable()
            .WithColumn("refreshtoken").AsString(500).Nullable()
            .WithColumn("refreshtokenexpiry").AsDateTime().Nullable()
            .WithColumn("description").AsString(255).Nullable()
            .WithColumn("comments").AsString(255).Nullable()
            .WithColumn("isactive").AsBoolean().WithDefaultValue(true)
            .WithColumn("createdby").AsInt32().Nullable()
            .WithColumn("createddate").AsDateTime().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("updatedby").AsInt32().Nullable()
            .WithColumn("updateddate").AsDateTime().WithDefault(SystemMethods.CurrentDateTime);
        
        Create.Table("rcims_roles")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("uuid").AsString(50).Unique()
            .WithColumn("name").AsString(100).NotNullable()
            .WithColumn("description").AsString(255).Nullable()
            .WithColumn("comments").AsString(255).Nullable()
            .WithColumn("isactive").AsBoolean().WithDefaultValue(true)
            .WithColumn("createdby").AsInt32().NotNullable()
                .ForeignKey("rcims_users", "id").OnDelete(System.Data.Rule.None)
            .WithColumn("createddate").AsDateTime().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("updatedby").AsInt32().NotNullable()
                .ForeignKey("rcims_users", "id").OnDelete(System.Data.Rule.None)
            .WithColumn("updateddate").AsDateTime().WithDefault(SystemMethods.CurrentDateTime);
        
        Create.Table("rcims_userroles")
            .WithColumn("userid").AsInt32().NotNullable()
                .ForeignKey("fk_userroles_users", "rcims_users", "id")
                .OnDelete(System.Data.Rule.None)
            .WithColumn("roleid").AsInt32().NotNullable()
                .ForeignKey("fk_userroles_roles", "rcims_roles", "id")
                .OnDelete(System.Data.Rule.None);
        
        Create.Table("rcims_accounts")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("uuid").AsString(50).Unique()
            .WithColumn("name").AsString(100).NotNullable()
            .WithColumn("accounttype").AsInt32().NotNullable()
            .WithColumn("openbalance").AsInt32().Nullable().WithDefaultValue(0)
            .WithColumn("unmatchedbalance").AsInt32().Nullable().WithDefaultValue(0)
            .WithColumn("description").AsString(255).Nullable()
            .WithColumn("comments").AsString(255).Nullable()
            .WithColumn("isactive").AsBoolean().WithDefaultValue(true)
            .WithColumn("createdby").AsInt32().NotNullable()
                .ForeignKey("rcims_users", "id").OnDelete(System.Data.Rule.None)
            .WithColumn("createddate").AsDateTime().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("updatedby").AsInt32().NotNullable()
                .ForeignKey("rcims_users", "id").OnDelete(System.Data.Rule.None)
            .WithColumn("updateddate").AsDateTime().WithDefault(SystemMethods.CurrentDateTime);
        
        Create.Table("rcims_plans")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("uuid").AsString(50).Unique()
            .WithColumn("name").AsString(100).NotNullable()
            .WithColumn("description").AsString(255).Nullable()
            .WithColumn("calculationtype").AsInt32().NotNullable()
            .WithColumn("months").AsInt32().Nullable()
            .WithColumn("equation").AsString(255).Nullable()
            .WithColumn("period").AsInt32().NotNullable()
            .WithColumn("percent").AsDecimal(18,3).NotNullable().WithDefaultValue(0)
            .WithColumn("maxallow").AsInt32().NotNullable()
            .WithColumn("isactive").AsBoolean().WithDefaultValue(true)
            .WithColumn("createdby").AsInt32().NotNullable()
            .ForeignKey("rcims_users", "id").OnDelete(System.Data.Rule.None)
            .WithColumn("createddate").AsDateTime().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("updatedby").AsInt32().NotNullable()
            .ForeignKey("rcims_users", "id").OnDelete(System.Data.Rule.None)
            .WithColumn("updateddate").AsDateTime().WithDefault(SystemMethods.CurrentDateTime);
        
        Create.Table("rcims_assets")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("uuid").AsString(50).Unique()
            .WithColumn("name").AsString(100).NotNullable()
            .WithColumn("description").AsString(255).Nullable()
            .WithColumn("assettype").AsInt32().NotNullable()
            .WithColumn("area").AsString(255).Nullable()
            .WithColumn("price").AsInt32().Nullable()
            .WithColumn("listprice").AsInt32().Nullable()
            .WithColumn("status").AsInt32().NotNullable()
            .WithColumn("isactive").AsBoolean().WithDefaultValue(true)
            .WithColumn("createdby").AsInt32().NotNullable()
            .ForeignKey("rcims_users", "id").OnDelete(System.Data.Rule.None)
            .WithColumn("createddate").AsDateTime().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("updatedby").AsInt32().NotNullable()
            .ForeignKey("rcims_users", "id").OnDelete(System.Data.Rule.None)
            .WithColumn("updateddate").AsDateTime().WithDefault(SystemMethods.CurrentDateTime);
        
        Create.Table("rcims_contracts")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("uuid").AsString(50).Unique()
            .WithColumn("contractnumber").AsString(10).NotNullable().Unique()
            .WithColumn("contractdate").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("contractstatus").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("description").AsString(255).Nullable()
            .WithColumn("contractfor").AsInt32().NotNullable()
                .ForeignKey("rcims_users", "id").OnDelete(System.Data.Rule.None)
            .WithColumn("asset").AsInt32().NotNullable()
                .ForeignKey("rcims_assets", "id").OnDelete(System.Data.Rule.None)
            .WithColumn("plan").AsInt32().NotNullable()
                .ForeignKey("rcims_plans", "id").OnDelete(System.Data.Rule.None)
            .WithColumn("totalamount").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("downpayment").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("paidamount").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("discount").AsDecimal(18,3).NotNullable().WithDefaultValue(0)
            .WithColumn("discountamount").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("gracedays").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("dailypenaltyrate").AsDecimal(9,6).NotNullable().WithDefaultValue(0)
            .WithColumn("earlypayoffdiscountpercent").AsDecimal(5,2).Nullable().WithDefaultValue(0)
            .WithColumn("subtotal").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("isactive").AsBoolean().WithDefaultValue(true)
            .WithColumn("createdby").AsInt32().NotNullable()
                .ForeignKey("rcims_users", "id").OnDelete(System.Data.Rule.None)
            .WithColumn("createddate").AsDateTime().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("updatedby").AsInt32().NotNullable()
                .ForeignKey("rcims_users", "id").OnDelete(System.Data.Rule.None)
            .WithColumn("updateddate").AsDateTime().WithDefault(SystemMethods.CurrentDateTime);
        
        Create.Table("rcims_installments")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("uuid").AsString(50).Unique()
            .WithColumn("contract").AsInt32().NotNullable()
                .ForeignKey("rcims_contracts", "id").OnDelete(System.Data.Rule.None)
            .WithColumn("duedate").AsDate().NotNullable()
            .WithColumn("status").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("amount").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("discount").AsDecimal(18,3).NotNullable().WithDefaultValue(0)
            .WithColumn("discountamount").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("penaltyaccrued").AsDecimal(18,2).NotNullable().WithDefaultValue(0)
            .WithColumn("subtotal").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("paidamount").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("description").AsString(500).Nullable()
            .WithColumn("isactive").AsBoolean().WithDefaultValue(true)
            .WithColumn("createdby").AsInt32().NotNullable()
                .ForeignKey("rcims_users", "id").OnDelete(System.Data.Rule.None)
            .WithColumn("createddate").AsDateTime().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("updatedby").AsInt32().NotNullable()
                .ForeignKey("rcims_users", "id").OnDelete(System.Data.Rule.None)
            .WithColumn("updateddate").AsDateTime().WithDefault(SystemMethods.CurrentDateTime);
        
        Create.Table("rcims_payments")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("uuid").AsString(50).Unique()
            .WithColumn("documentnumber").AsInt32().NotNullable()
            .WithColumn("documentdate").AsDateTime().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("paymentmethod").AsInt32().NotNullable()
            .WithColumn("paymenttype").AsInt32().NotNullable()
            .WithColumn("contractid").AsInt32().Nullable()
                .ForeignKey("rcims_contracts", "id").OnDelete(System.Data.Rule.None)
            .WithColumn("installmentid").AsInt32().Nullable()
                .ForeignKey("rcims_contractinstallments", "id").OnDelete(System.Data.Rule.None)
            .WithColumn("amount").AsInt32().NotNullable()
            .WithColumn("currency").AsInt32().NotNullable()
            .WithColumn("documentstatus").AsInt32().NotNullable()
            .WithColumn("documentfor").AsInt32().NotNullable()
                .ForeignKey("rcims_users", "id").OnDelete(System.Data.Rule.None)
            .WithColumn("accountid").AsInt32().NotNullable()
                .ForeignKey("rcims_accounts", "id").OnDelete(System.Data.Rule.None)
            .WithColumn("description").AsString(255).Nullable()
            .WithColumn("comments").AsString(255).Nullable()
            .WithColumn("isactive").AsBoolean().WithDefaultValue(true)
            .WithColumn("createdby").AsInt32().NotNullable()
                .ForeignKey("rcims_users", "id").OnDelete(System.Data.Rule.None)
            .WithColumn("createddate").AsDateTime().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("updatedby").AsInt32().NotNullable()
                .ForeignKey("rcims_users", "id").OnDelete(System.Data.Rule.None)
            .WithColumn("updateddate").AsDateTime().WithDefault(SystemMethods.CurrentDateTime);
            
            
    }
}