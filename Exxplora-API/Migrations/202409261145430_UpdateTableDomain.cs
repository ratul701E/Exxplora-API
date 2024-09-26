namespace Exxplora_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTableDomain : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DomainProjects", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.DomainProjects", "Domain_Id", "dbo.Domains");
            DropIndex("dbo.DomainProjects", new[] { "Project_Id" });
            DropIndex("dbo.DomainProjects", new[] { "Domain_Id" });
            AddColumn("dbo.Domains", "Project_Id", c => c.Int());
            CreateIndex("dbo.Domains", "Project_Id");
            AddForeignKey("dbo.Domains", "Project_Id", "dbo.Projects", "Id");
            DropTable("dbo.DomainProjects");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DomainProjects",
                c => new
                    {
                        Project_Id = c.Int(nullable: false),
                        Domain_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.Domain_Id });
            
            DropForeignKey("dbo.Domains", "Project_Id", "dbo.Projects");
            DropIndex("dbo.Domains", new[] { "Project_Id" });
            DropColumn("dbo.Domains", "Project_Id");
            CreateIndex("dbo.DomainProjects", "Domain_Id");
            CreateIndex("dbo.DomainProjects", "Project_Id");
            AddForeignKey("dbo.DomainProjects", "Domain_Id", "dbo.Domains", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DomainProjects", "Project_Id", "dbo.Projects", "Id", cascadeDelete: true);
        }
    }
}
