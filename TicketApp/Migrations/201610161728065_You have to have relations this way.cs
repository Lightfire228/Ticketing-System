namespace TicketApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Youhavetohaverelationsthisway : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ChatAppointments", newName: "Chats");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Chats", newName: "ChatAppointments");
        }
    }
}
