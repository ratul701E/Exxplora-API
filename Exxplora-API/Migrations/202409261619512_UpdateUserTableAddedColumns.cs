namespace Exxplora_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserTableAddedColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "FirstName", c => c.String(nullable: false, maxLength: 32));
            AddColumn("dbo.Users", "LastName", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("dbo.Users", "Phone", c => c.String());
            AlterColumn("dbo.Users", "Institute", c => c.String());
            DropColumn("dbo.Users", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Name", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("dbo.Users", "Institute", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Users", "Phone", c => c.String(maxLength: 15));
            DropColumn("dbo.Users", "LastName");
            DropColumn("dbo.Users", "FirstName");
        }
    }
}
