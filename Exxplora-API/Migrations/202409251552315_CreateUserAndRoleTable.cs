namespace Exxplora_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateUserAndRoleTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserModels", "StartYear", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserModels", "StartYear", c => c.DateTime(nullable: false));
        }
    }
}
