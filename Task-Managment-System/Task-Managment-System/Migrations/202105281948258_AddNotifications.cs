namespace Task_Managment_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotifications : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        TaskId = c.Int(),
                        ProjectId = c.Int(),
                        Title = c.String(),
                        Contents = c.String(),
                        Read = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .ForeignKey("dbo.ProjectTasks", t => t.TaskId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.TaskId)
                .Index(t => t.ProjectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notifications", "TaskId", "dbo.ProjectTasks");
            DropForeignKey("dbo.Notifications", "ProjectId", "dbo.Projects");
            DropIndex("dbo.Notifications", new[] { "ProjectId" });
            DropIndex("dbo.Notifications", new[] { "TaskId" });
            DropIndex("dbo.Notifications", new[] { "UserId" });
            DropTable("dbo.Notifications");
        }
    }
}
