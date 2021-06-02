namespace Task_Managment_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentClassAddTitlePriortiyNullable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "Title", c => c.String());
            AlterColumn("dbo.Comments", "Priority", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Comments", "Priority", c => c.Int(nullable: false));
            DropColumn("dbo.Comments", "Title");
        }
    }
}
