namespace Task_Managment_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPriorityToTask : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectTasks", "Priority", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectTasks", "Priority");
        }
    }
}
