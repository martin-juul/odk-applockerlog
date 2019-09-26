namespace AppLockerLog.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            /*CreateTable(
                "dbo.AppLockerLogEntries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        ComputerName = c.String(),
                        Ip = c.String(),
                        ProgramDescription = c.String(),
                        RapportDescription = c.String(),
                        TimeStamp = c.DateTime(nullable: false),
                        EditedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);*/
            
        }
        
        public override void Down()
        {
            // DropTable("dbo.AppLockerLogEntries");
        }
    }
}
