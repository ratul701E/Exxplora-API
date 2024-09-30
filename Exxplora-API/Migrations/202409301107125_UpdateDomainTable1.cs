namespace Exxplora_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDomainTable1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Domains", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Domains", "Description", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Domains", "Description", c => c.String());
            AlterColumn("dbo.Domains", "Name", c => c.String());
        }
    }
}
