namespace Exxplora_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTableSection : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sections", "CreatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.Sections", "User_ID", c => c.Int());
            CreateIndex("dbo.Sections", "User_ID");
            AddForeignKey("dbo.Sections", "User_ID", "dbo.Users", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sections", "User_ID", "dbo.Users");
            DropIndex("dbo.Sections", new[] { "User_ID" });
            DropColumn("dbo.Sections", "User_ID");
            DropColumn("dbo.Sections", "CreatedBy");
        }
    }
}
