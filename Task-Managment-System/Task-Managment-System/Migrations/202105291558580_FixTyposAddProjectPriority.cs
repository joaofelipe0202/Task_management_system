namespace Task_Managment_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixTyposAddProjectPriority : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "DailySalary", c => c.Double());
            AddColumn("dbo.Projects", "Priority", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "DailySalaray");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "DailySalaray", c => c.Double());
            DropColumn("dbo.Projects", "Priority");
            DropColumn("dbo.AspNetUsers", "DailySalary");
        }
    }
}
