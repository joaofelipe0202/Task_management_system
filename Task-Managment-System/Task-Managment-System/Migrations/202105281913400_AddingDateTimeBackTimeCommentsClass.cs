namespace Task_Managment_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingDateTimeBackTimeCommentsClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "DateCreated", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "DateCreated");
        }
    }
}
