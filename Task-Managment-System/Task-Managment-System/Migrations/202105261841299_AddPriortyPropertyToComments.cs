namespace Task_Managment_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPriortyPropertyToComments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "Priority", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "Priority");
        }
    }
}
