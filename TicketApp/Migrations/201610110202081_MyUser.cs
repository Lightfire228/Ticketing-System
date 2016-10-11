namespace TicketApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MyUser : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Users", newName: "MyUsers");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.MyUsers", newName: "Users");
        }
    }
}
