namespace Exxplora_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateProjectTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Projects", "Budget");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "Budget", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
