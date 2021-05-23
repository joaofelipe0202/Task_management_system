namespace Task_Managment_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetDailySalaryNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "DailySalaray", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "DailySalaray", c => c.Double(nullable: false));
        }
    }
}
