namespace Exxplora_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserTableAddedColumns1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "FirstName", c => c.String(maxLength: 32));
            AlterColumn("dbo.Users", "LastName", c => c.String(maxLength: 32));
            AlterColumn("dbo.Users", "Phone", c => c.String(maxLength: 15));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Phone", c => c.String());
            AlterColumn("dbo.Users", "LastName", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("dbo.Users", "FirstName", c => c.String(nullable: false, maxLength: 32));
        }
    }
}
