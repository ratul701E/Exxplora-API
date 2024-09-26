namespace Exxplora_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTableNames : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.RoleModels", newName: "Roles");
            RenameTable(name: "dbo.UserModels", newName: "Users");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Users", newName: "UserModels");
            RenameTable(name: "dbo.Roles", newName: "RoleModels");
        }
    }
}
