namespace Task_Managment_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterprojecttaskmodel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProjectTasks", "Project_Id", "dbo.Projects");
            DropIndex("dbo.ProjectTasks", new[] { "Project_Id" });
            RenameColumn(table: "dbo.ProjectTasks", name: "Project_Id", newName: "ProjectId");
            AddColumn("dbo.Projects", "Description", c => c.String());
            AlterColumn("dbo.ProjectTasks", "ProjectId", c => c.Int(nullable: false));
            CreateIndex("dbo.ProjectTasks", "ProjectId");
            AddForeignKey("dbo.ProjectTasks", "ProjectId", "dbo.Projects", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectTasks", "ProjectId", "dbo.Projects");
            DropIndex("dbo.ProjectTasks", new[] { "ProjectId" });
            AlterColumn("dbo.ProjectTasks", "ProjectId", c => c.Int());
            DropColumn("dbo.Projects", "Description");
            RenameColumn(table: "dbo.ProjectTasks", name: "ProjectId", newName: "Project_Id");
            CreateIndex("dbo.ProjectTasks", "Project_Id");
            AddForeignKey("dbo.ProjectTasks", "Project_Id", "dbo.Projects", "Id");
        }
    }
}
