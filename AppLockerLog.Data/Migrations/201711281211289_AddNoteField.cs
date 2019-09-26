namespace AppLockerLog.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNoteField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppLockerLogEntries", "Note", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppLockerLogEntries", "Note");
        }
    }
}
