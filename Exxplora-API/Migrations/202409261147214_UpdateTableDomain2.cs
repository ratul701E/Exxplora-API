namespace Exxplora_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTableDomain2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Domains", "Project_Id", "dbo.Projects");
            DropIndex("dbo.Domains", new[] { "Project_Id" });
            CreateTable(
                "dbo.ProjectDomains",
                c => new
                    {
                        Project_Id = c.Int(nullable: false),
                        Domain_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.Domain_Id })
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .ForeignKey("dbo.Domains", t => t.Domain_Id, cascadeDelete: true)
                .Index(t => t.Project_Id)
                .Index(t => t.Domain_Id);
            
            DropColumn("dbo.Domains", "Project_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Domains", "Project_Id", c => c.Int());
            DropForeignKey("dbo.ProjectDomains", "Domain_Id", "dbo.Domains");
            DropForeignKey("dbo.ProjectDomains", "Project_Id", "dbo.Projects");
            DropIndex("dbo.ProjectDomains", new[] { "Domain_Id" });
            DropIndex("dbo.ProjectDomains", new[] { "Project_Id" });
            DropTable("dbo.ProjectDomains");
            CreateIndex("dbo.Domains", "Project_Id");
            AddForeignKey("dbo.Domains", "Project_Id", "dbo.Projects", "Id");
        }
    }
}
