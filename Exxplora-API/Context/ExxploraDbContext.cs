using Exxplora_API.Models;
using System.Data.Entity;
using System.Linq;

namespace Exxplora_API.Context
{
    public class ExxploraDbContext : DbContext
    {
        // Your context has been configured to use a 'ExxploraDbContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Exxplora_API.Context.ExxploraDbContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'ExxploraDbContext' 
        // connection string in the application configuration file.
        public ExxploraDbContext()
            : base("name=ExxploraDbContext")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectStatus> ProjectStatuses { get; set; }
        public virtual DbSet<Domain> Domains { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<Version> Versions { get; set; }
    }



    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}