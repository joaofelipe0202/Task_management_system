namespace Task_Managment_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommentClassAndRelations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        CreatorId = c.String(maxLength: 128),
                        TaskId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        ProjectTask_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorId)
                .ForeignKey("dbo.ProjectTasks", t => t.ProjectTask_Id)
                .Index(t => t.CreatorId)
                .Index(t => t.ProjectTask_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "ProjectTask_Id", "dbo.ProjectTasks");
            DropForeignKey("dbo.Comments", "CreatorId", "dbo.AspNetUsers");
            DropIndex("dbo.Comments", new[] { "ProjectTask_Id" });
            DropIndex("dbo.Comments", new[] { "CreatorId" });
            DropTable("dbo.Comments");
        }
    }
}
