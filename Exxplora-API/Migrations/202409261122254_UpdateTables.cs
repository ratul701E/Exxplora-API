namespace Exxplora_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTables : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.DomainProjects", newName: "DomainProjects");
            DropForeignKey("dbo.Users", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.DomainProjects", "Project_Id", "dbo.Projects");
            DropIndex("dbo.Users", new[] { "Project_Id" });
            DropIndex("dbo.DomainProjects", new[] { "Project_Id" });
            DropPrimaryKey("dbo.Projects");
            DropPrimaryKey("dbo.DomainProjects");
            AlterColumn("dbo.Users", "Project_Id", c => c.Int());
            AlterColumn("dbo.Projects", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.DomainProjects", "Project_Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Projects", "Id");
            AddPrimaryKey("dbo.DomainProjects", new[] { "Project_Id", "Domain_Id" });
            CreateIndex("dbo.Users", "Project_Id");
            CreateIndex("dbo.DomainProjects", "Project_Id");
            AddForeignKey("dbo.Users", "Project_Id", "dbo.Projects", "Id");
            AddForeignKey("dbo.DomainProjects", "Project_Id", "dbo.Projects", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DomainProjects", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Users", "Project_Id", "dbo.Projects");
            DropIndex("dbo.DomainProjects", new[] { "Project_Id" });
            DropIndex("dbo.Users", new[] { "Project_Id" });
            DropPrimaryKey("dbo.DomainProjects");
            DropPrimaryKey("dbo.Projects");
            AlterColumn("dbo.DomainProjects", "Project_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Projects", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Users", "Project_Id", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.DomainProjects", new[] { "Domain_Id", "Project_Id" });
            AddPrimaryKey("dbo.Projects", "Id");
            CreateIndex("dbo.DomainProjects", "Project_Id");
            CreateIndex("dbo.Users", "Project_Id");
            AddForeignKey("dbo.DomainProjects", "Project_Id", "dbo.Projects", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Users", "Project_Id", "dbo.Projects", "Id");
            RenameTable(name: "dbo.DomainProjects", newName: "DomainProjects");
        }
    }
}
