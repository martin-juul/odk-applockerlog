namespace AppLockerLog.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeniedByField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppLockerLogEntries", "DeniedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppLockerLogEntries", "DeniedBy");
        }
    }
}
