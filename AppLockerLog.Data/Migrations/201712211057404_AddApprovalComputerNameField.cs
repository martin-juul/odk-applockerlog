namespace AppLockerLog.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddApprovalComputerNameField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Approval", "ComputerName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Approval", "ComputerName");
        }
    }
}
