namespace TicketApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class thj : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatAppointments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ChatToUsers",
                c => new
                    {
                        ChatID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ChatID, t.UserID })
                .ForeignKey("dbo.ChatAppointments", t => t.ChatID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.ChatID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.TicketComponents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        TicketID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        Text = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Tickets", t => t.TicketID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.TicketID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.UsersTickets",
                c => new
                    {
                        UserID = c.Int(nullable: false),
                        TicketID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserID, t.TicketID })
                .ForeignKey("dbo.Tickets", t => t.TicketID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.TicketID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsersTickets", "UserID", "dbo.Users");
            DropForeignKey("dbo.UsersTickets", "TicketID", "dbo.Tickets");
            DropForeignKey("dbo.TicketComponents", "UserID", "dbo.Users");
            DropForeignKey("dbo.TicketComponents", "TicketID", "dbo.Tickets");
            DropForeignKey("dbo.ChatToUsers", "UserID", "dbo.Users");
            DropForeignKey("dbo.ChatToUsers", "ChatID", "dbo.ChatAppointments");
            DropIndex("dbo.UsersTickets", new[] { "TicketID" });
            DropIndex("dbo.UsersTickets", new[] { "UserID" });
            DropIndex("dbo.TicketComponents", new[] { "UserID" });
            DropIndex("dbo.TicketComponents", new[] { "TicketID" });
            DropIndex("dbo.ChatToUsers", new[] { "UserID" });
            DropIndex("dbo.ChatToUsers", new[] { "ChatID" });
            DropTable("dbo.UsersTickets");
            DropTable("dbo.TicketComponents");
            DropTable("dbo.ChatToUsers");
            DropTable("dbo.ChatAppointments");
        }
    }
}
