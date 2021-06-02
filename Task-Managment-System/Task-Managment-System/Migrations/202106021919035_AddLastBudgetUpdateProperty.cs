namespace Task_Managment_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLastBudgetUpdateProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "LastBudgetUpdate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "LastBudgetUpdate");
        }
    }
}
