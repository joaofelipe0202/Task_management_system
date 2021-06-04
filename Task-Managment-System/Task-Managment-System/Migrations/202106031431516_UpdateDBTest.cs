namespace Task_Managment_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDBTest : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Projects", "CreatorId", "dbo.AspNetUsers");
            DropIndex("dbo.Projects", new[] { "CreatorId" });
            AddColumn("dbo.Comments", "Priority", c => c.Int(nullable: false));
            AlterColumn("dbo.Projects", "CreatorId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Projects", "CreatorId");
            AddForeignKey("dbo.Projects", "CreatorId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Comments", "Title");
            DropColumn("dbo.Comments", "IsUrgent");
            DropColumn("dbo.Projects", "LastBudgetUpdate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "LastBudgetUpdate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Comments", "IsUrgent", c => c.Boolean(nullable: false));
            AddColumn("dbo.Comments", "Title", c => c.String());
            DropForeignKey("dbo.Projects", "CreatorId", "dbo.AspNetUsers");
            DropIndex("dbo.Projects", new[] { "CreatorId" });
            AlterColumn("dbo.Projects", "CreatorId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Comments", "Priority");
            CreateIndex("dbo.Projects", "CreatorId");
            AddForeignKey("dbo.Projects", "CreatorId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
