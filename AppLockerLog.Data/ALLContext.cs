using AppLockerLog.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLockerLog.Data.Repository
{
    public class ALLContext : DbContext
    {
        public ALLContext()
             : base("DefaultConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion
                <ALLContext, AppLockerLogContextMigrationConfiguration>());
        }

        public DbSet<LogEntry> LogEntries { get; set; }
        public DbSet<Approval> Approvals { get; set; }
        public DbSet<AssignedUserGroup> AssignedUserGroups { get; set; }
        public DbSet<SoftwareEntry> SoftwareEntries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
