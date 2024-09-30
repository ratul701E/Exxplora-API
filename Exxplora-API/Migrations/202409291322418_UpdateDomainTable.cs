namespace Exxplora_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDomainTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Domains", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Domains", "Description");
        }
    }
}
