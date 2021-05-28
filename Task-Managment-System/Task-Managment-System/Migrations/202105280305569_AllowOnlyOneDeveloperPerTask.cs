namespace Task_Managment_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllowOnlyOneDeveloperPerTask : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "ProjectTask_Id", "dbo.ProjectTasks");
            DropIndex("dbo.AspNetUsers", new[] { "ProjectTask_Id" });
            AddColumn("dbo.ProjectTasks", "AssignedUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.ProjectTasks", "AssignedUser_Id");
            AddForeignKey("dbo.ProjectTasks", "AssignedUser_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.AspNetUsers", "ProjectTask_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "ProjectTask_Id", c => c.Int());
            DropForeignKey("dbo.ProjectTasks", "AssignedUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ProjectTasks", new[] { "AssignedUser_Id" });
            DropColumn("dbo.ProjectTasks", "AssignedUser_Id");
            CreateIndex("dbo.AspNetUsers", "ProjectTask_Id");
            AddForeignKey("dbo.AspNetUsers", "ProjectTask_Id", "dbo.ProjectTasks", "Id");
        }
    }
}
