namespace Task_Managment_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatingHideCompletedTasksView : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Projects", "CreatorId", "dbo.AspNetUsers");
            DropIndex("dbo.Projects", new[] { "CreatorId" });
            AlterColumn("dbo.Projects", "CreatorId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Projects", "CreatorId");
            AddForeignKey("dbo.Projects", "CreatorId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.Projects", "PercentageCompleted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "PercentageCompleted", c => c.Int(nullable: false));
            DropForeignKey("dbo.Projects", "CreatorId", "dbo.AspNetUsers");
            DropIndex("dbo.Projects", new[] { "CreatorId" });
            AlterColumn("dbo.Projects", "CreatorId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Projects", "CreatorId");
            AddForeignKey("dbo.Projects", "CreatorId", "dbo.AspNetUsers", "Id");
        }
    }
}
