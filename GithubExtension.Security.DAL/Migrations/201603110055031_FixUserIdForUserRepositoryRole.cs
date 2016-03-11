namespace GithubExtension.Security.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixUserIdForUserRepositoryRole : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.UserRepositoryRoles", new[] { "User_Id" });
            DropColumn("dbo.UserRepositoryRoles", "UserId");
            RenameColumn(table: "dbo.UserRepositoryRoles", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.UserRepositoryRoles", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.UserRepositoryRoles", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserRepositoryRoles", new[] { "UserId" });
            AlterColumn("dbo.UserRepositoryRoles", "UserId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.UserRepositoryRoles", name: "UserId", newName: "User_Id");
            AddColumn("dbo.UserRepositoryRoles", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.UserRepositoryRoles", "User_Id");
        }
    }
}
