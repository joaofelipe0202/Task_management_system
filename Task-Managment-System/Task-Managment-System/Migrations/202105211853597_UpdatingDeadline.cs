namespace Task_Managment_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatingDeadline : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "Deadline", c => c.DateTime(nullable: false));
            AddColumn("dbo.ProjectTasks", "Deadline", c => c.DateTime(nullable: false));
            DropColumn("dbo.Projects", "DueDate");
            DropColumn("dbo.ProjectTasks", "DueDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProjectTasks", "DueDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Projects", "DueDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.ProjectTasks", "Deadline");
            DropColumn("dbo.Projects", "Deadline");
        }
    }
}
