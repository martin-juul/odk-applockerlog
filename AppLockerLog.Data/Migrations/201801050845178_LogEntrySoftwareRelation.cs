namespace AppLockerLog.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LogEntrySoftwareRelation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppLockerLogEntries", "Software_Id", c => c.Int(nullable: true));
            CreateIndex("dbo.AppLockerLogEntries", "Software_Id");
            AddForeignKey("dbo.AppLockerLogEntries", "Software_Id", "dbo.SoftwareEntry", "Id", cascadeDelete: false);
            DropColumn("dbo.AppLockerLogEntries", "SoftwareName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppLockerLogEntries", "SoftwareName", c => c.String());
            DropForeignKey("dbo.AppLockerLogEntries", "Software_Id", "dbo.SoftwareEntry");
            DropIndex("dbo.AppLockerLogEntries", new[] { "Software_Id" });
            DropColumn("dbo.AppLockerLogEntries", "Software_Id");
        }
    }
}
