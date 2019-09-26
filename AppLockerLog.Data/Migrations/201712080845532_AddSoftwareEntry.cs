namespace AppLockerLog.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSoftwareEntry : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SoftwareEntry",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Vendor = c.String(),
                        Reasoning = c.String(),
                        TimeStamp = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        State = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SoftwareEntry");
        }
    }
}
