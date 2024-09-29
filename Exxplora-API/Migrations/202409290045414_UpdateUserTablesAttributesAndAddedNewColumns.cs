namespace Exxplora_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserTablesAttributesAndAddedNewColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Domains", "User_ID", c => c.Int());
            AddColumn("dbo.Users", "EndYear", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "Location", c => c.String());
            CreateIndex("dbo.Domains", "User_ID");
            AddForeignKey("dbo.Domains", "User_ID", "dbo.Users", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Domains", "User_ID", "dbo.Users");
            DropIndex("dbo.Domains", new[] { "User_ID" });
            DropColumn("dbo.Users", "Location");
            DropColumn("dbo.Users", "EndYear");
            DropColumn("dbo.Domains", "User_ID");
        }
    }
}
