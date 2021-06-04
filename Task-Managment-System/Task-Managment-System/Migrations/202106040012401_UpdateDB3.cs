namespace Task_Managment_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDB3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Projects", "CreatorId", "dbo.AspNetUsers");
            DropIndex("dbo.Projects", new[] { "CreatorId" });
            AddColumn("dbo.Comments", "Title", c => c.String());
            AddColumn("dbo.Comments", "IsUrgent", c => c.Boolean(nullable: false));
            AddColumn("dbo.Projects", "LastBudgetUpdate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Projects", "CreatorId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Projects", "CreatorId");
            AddForeignKey("dbo.Projects", "CreatorId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.Comments", "Priority");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "Priority", c => c.Int(nullable: false));
            DropForeignKey("dbo.Projects", "CreatorId", "dbo.AspNetUsers");
            DropIndex("dbo.Projects", new[] { "CreatorId" });
            AlterColumn("dbo.Projects", "CreatorId", c => c.String(maxLength: 128));
            DropColumn("dbo.Projects", "LastBudgetUpdate");
            DropColumn("dbo.Comments", "IsUrgent");
            DropColumn("dbo.Comments", "Title");
            CreateIndex("dbo.Projects", "CreatorId");
            AddForeignKey("dbo.Projects", "CreatorId", "dbo.AspNetUsers", "Id");
        }
    }
}
