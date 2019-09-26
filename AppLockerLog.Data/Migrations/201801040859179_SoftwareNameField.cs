namespace AppLockerLog.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SoftwareNameField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppLockerLogEntries", "SoftwareName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppLockerLogEntries", "SoftwareName");
        }
    }
}
