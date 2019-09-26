using AppLockerLog.Data.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLockerLog.Data
{
    class AppLockerLogContextMigrationConfiguration : DbMigrationsConfiguration<ALLContext>
    {
        public AppLockerLogContextMigrationConfiguration()
        {
            this.AutomaticMigrationsEnabled = false;
            this.AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(ALLContext context)
        {
#if DEBUG
            // new AppLockerLogDataSeeder(context).Seed();
#endif
        }
    }
}
