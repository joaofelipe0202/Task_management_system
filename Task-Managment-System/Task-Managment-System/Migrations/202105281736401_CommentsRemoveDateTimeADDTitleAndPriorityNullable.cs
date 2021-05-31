namespace Task_Managment_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentsRemoveDateTimeADDTitleAndPriorityNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "ProjectTask_Id", "dbo.ProjectTasks");
            DropIndex("dbo.Comments", new[] { "ProjectTask_Id" });
            RenameColumn(table: "dbo.Comments", name: "ProjectTask_Id", newName: "ProjectTaskId");
            AddColumn("dbo.Comments", "Title", c => c.String());
            AlterColumn("dbo.Comments", "Priority", c => c.Int());
            AlterColumn("dbo.Comments", "ProjectTaskId", c => c.Int(nullable: false));
            CreateIndex("dbo.Comments", "ProjectTaskId");
            AddForeignKey("dbo.Comments", "ProjectTaskId", "dbo.ProjectTasks", "Id", cascadeDelete: true);
            DropColumn("dbo.Comments", "TaskId");
            DropColumn("dbo.Comments", "DateCreated");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Comments", "TaskId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Comments", "ProjectTaskId", "dbo.ProjectTasks");
            DropIndex("dbo.Comments", new[] { "ProjectTaskId" });
            AlterColumn("dbo.Comments", "ProjectTaskId", c => c.Int());
            AlterColumn("dbo.Comments", "Priority", c => c.Int(nullable: false));
            DropColumn("dbo.Comments", "Title");
            RenameColumn(table: "dbo.Comments", name: "ProjectTaskId", newName: "ProjectTask_Id");
            CreateIndex("dbo.Comments", "ProjectTask_Id");
            AddForeignKey("dbo.Comments", "ProjectTask_Id", "dbo.ProjectTasks", "Id");
        }
    }
}
