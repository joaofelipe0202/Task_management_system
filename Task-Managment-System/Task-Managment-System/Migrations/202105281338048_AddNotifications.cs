namespace Task_Managment_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotifications : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "ProjectTask_Id", "dbo.ProjectTasks");
            DropIndex("dbo.AspNetUsers", new[] { "ProjectTask_Id" });
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
            
            AddColumn("dbo.ProjectTasks", "AssignedUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.ProjectTasks", "AssignedUser_Id");
            AddForeignKey("dbo.ProjectTasks", "AssignedUser_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.AspNetUsers", "ProjectTask_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "ProjectTask_Id", c => c.Int());
            DropForeignKey("dbo.Notifications", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notifications", "TaskId", "dbo.ProjectTasks");
            DropForeignKey("dbo.Notifications", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ProjectTasks", "AssignedUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ProjectTasks", new[] { "AssignedUser_Id" });
            DropIndex("dbo.Notifications", new[] { "ProjectId" });
            DropIndex("dbo.Notifications", new[] { "TaskId" });
            DropIndex("dbo.Notifications", new[] { "UserId" });
            DropColumn("dbo.ProjectTasks", "AssignedUser_Id");
            DropTable("dbo.Notifications");
            CreateIndex("dbo.AspNetUsers", "ProjectTask_Id");
            AddForeignKey("dbo.AspNetUsers", "ProjectTask_Id", "dbo.ProjectTasks", "Id");
        }
    }
}
