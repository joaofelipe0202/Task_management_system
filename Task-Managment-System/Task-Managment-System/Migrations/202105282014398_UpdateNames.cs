namespace Task_Managment_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateNames : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ProjectTasks", name: "AssignedUser_Id", newName: "AssignedUserId");
            RenameIndex(table: "dbo.ProjectTasks", name: "IX_AssignedUser_Id", newName: "IX_AssignedUserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.ProjectTasks", name: "IX_AssignedUserId", newName: "IX_AssignedUser_Id");
            RenameColumn(table: "dbo.ProjectTasks", name: "AssignedUserId", newName: "AssignedUser_Id");
        }
    }
}
