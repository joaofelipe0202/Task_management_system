namespace Task_Managment_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePrioritiesAddIsUrgentToCommentClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "IsUrgent", c => c.Boolean(nullable: false));
            DropColumn("dbo.Comments", "Priority");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "Priority", c => c.Int());
            DropColumn("dbo.Comments", "IsUrgent");
        }
    }
}
