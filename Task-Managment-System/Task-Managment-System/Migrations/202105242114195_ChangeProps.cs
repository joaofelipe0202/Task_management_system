namespace Task_Managment_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeProps : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "Percentage", c => c.Int(nullable: false));
            AddColumn("dbo.Projects", "ActualCost", c => c.Double(nullable: false));
            AddColumn("dbo.Projects", "Complete", c => c.Boolean(nullable: false));
            AddColumn("dbo.Projects", "PercentageCompleted", c => c.Int(nullable: false));
            AddColumn("dbo.ProjectTasks", "PercentageCompleted", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectTasks", "PercentageCompleted");
            DropColumn("dbo.Projects", "PercentageCompleted");
            DropColumn("dbo.Projects", "Complete");
            DropColumn("dbo.Projects", "ActualCost");
            DropColumn("dbo.Projects", "Percentage");
        }
    }
}
