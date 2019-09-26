namespace AppLockerLog.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApprovalAndUserGroups : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Approval",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Reasoning = c.String(),
                        TimeStamp = c.DateTime(nullable: false),
                        Approver = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AssignedUserGroup",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ApprovalID = c.Int(nullable: false),
                        Group = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Approval", t => t.ApprovalID, cascadeDelete: true)
                .Index(t => t.ApprovalID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssignedUserGroup", "ApprovalID", "dbo.Approval");
            DropIndex("dbo.AssignedUserGroup", new[] { "ApprovalID" });
            DropTable("dbo.AssignedUserGroup");
            DropTable("dbo.Approval");
        }
    }
}
