namespace Exxplora_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserTableAddColumnProfilePathAndCoverPath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ProfilePicturePath", c => c.String());
            AddColumn("dbo.Users", "CoverPhotoPath", c => c.String());
            AlterColumn("dbo.Users", "Institute", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Institute", c => c.String());
            DropColumn("dbo.Users", "CoverPhotoPath");
            DropColumn("dbo.Users", "ProfilePicturePath");
        }
    }
}
