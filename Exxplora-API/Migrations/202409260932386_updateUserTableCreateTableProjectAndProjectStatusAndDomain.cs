namespace Exxplora_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateUserTableCreateTableProjectAndProjectStatusAndDomain : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        AuthorId = c.Int(nullable: false),
                        ProjectStatusId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        Budget = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsArchived = c.Boolean(nullable: false),
                        User_ID = c.Int(),
                        User_ID1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.ProjectStatus", t => t.ProjectStatusId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_ID)
                .ForeignKey("dbo.Users", t => t.User_ID1)
                .Index(t => t.AuthorId)
                .Index(t => t.ProjectStatusId)
                .Index(t => t.User_ID)
                .Index(t => t.User_ID1);
            
            CreateTable(
                "dbo.Domains",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DomainProjects",
                c => new
                    {
                        Domain_Id = c.Int(nullable: false),
                        Project_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Domain_Id, t.Project_Id })
                .ForeignKey("dbo.Domains", t => t.Domain_Id, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .Index(t => t.Domain_Id)
                .Index(t => t.Project_Id);
            
            AddColumn("dbo.Users", "Project_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Users", "Project_Id");
            AddForeignKey("dbo.Users", "Project_Id", "dbo.Projects", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "User_ID1", "dbo.Users");
            DropForeignKey("dbo.Projects", "User_ID", "dbo.Users");
            DropForeignKey("dbo.Projects", "ProjectStatusId", "dbo.ProjectStatus");
            DropForeignKey("dbo.DomainProjects", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.DomainProjects", "Domain_Id", "dbo.Domains");
            DropForeignKey("dbo.Users", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Projects", "AuthorId", "dbo.Users");
            DropIndex("dbo.DomainProjects", new[] { "Project_Id" });
            DropIndex("dbo.DomainProjects", new[] { "Domain_Id" });
            DropIndex("dbo.Projects", new[] { "User_ID1" });
            DropIndex("dbo.Projects", new[] { "User_ID" });
            DropIndex("dbo.Projects", new[] { "ProjectStatusId" });
            DropIndex("dbo.Projects", new[] { "AuthorId" });
            DropIndex("dbo.Users", new[] { "Project_Id" });
            DropColumn("dbo.Users", "Project_Id");
            DropTable("dbo.DomainProjects");
            DropTable("dbo.ProjectStatus");
            DropTable("dbo.Domains");
            DropTable("dbo.Projects");
        }
    }
}
