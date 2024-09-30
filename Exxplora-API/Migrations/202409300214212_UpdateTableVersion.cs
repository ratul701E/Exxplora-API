namespace Exxplora_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTableVersion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Versions", "CreatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.Versions", "User_ID", c => c.Int());
            CreateIndex("dbo.Versions", "User_ID");
            AddForeignKey("dbo.Versions", "User_ID", "dbo.Users", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Versions", "User_ID", "dbo.Users");
            DropIndex("dbo.Versions", new[] { "User_ID" });
            DropColumn("dbo.Versions", "User_ID");
            DropColumn("dbo.Versions", "CreatedBy");
        }
    }
}
