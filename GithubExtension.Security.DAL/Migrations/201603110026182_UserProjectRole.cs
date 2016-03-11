namespace GithubExtension.Security.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserProjectRole : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Repositories", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserRepositoryRoles", "Repository_Id", "dbo.Repositories");
            DropIndex("dbo.Repositories", new[] { "User_Id" });
            DropIndex("dbo.UserRepositoryRoles", new[] { "Repository_Id" });
            RenameColumn(table: "dbo.UserRepositoryRoles", name: "Repository_Id", newName: "RepositoryId");
            AlterColumn("dbo.UserRepositoryRoles", "RepositoryId", c => c.Int(nullable: false));
            AlterColumn("dbo.SecurityRoles", "Name", c => c.String());
            CreateIndex("dbo.UserRepositoryRoles", "RepositoryId");
            AddForeignKey("dbo.UserRepositoryRoles", "RepositoryId", "dbo.Repositories", "Id", cascadeDelete: true);
            DropColumn("dbo.Repositories", "User_Id");
            DropColumn("dbo.UserRepositoryRoles", "ProjectId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserRepositoryRoles", "ProjectId", c => c.Int(nullable: false));
            AddColumn("dbo.Repositories", "User_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.UserRepositoryRoles", "RepositoryId", "dbo.Repositories");
            DropIndex("dbo.UserRepositoryRoles", new[] { "RepositoryId" });
            AlterColumn("dbo.SecurityRoles", "Name", c => c.Int(nullable: false));
            AlterColumn("dbo.UserRepositoryRoles", "RepositoryId", c => c.Int());
            RenameColumn(table: "dbo.UserRepositoryRoles", name: "RepositoryId", newName: "Repository_Id");
            CreateIndex("dbo.UserRepositoryRoles", "Repository_Id");
            CreateIndex("dbo.Repositories", "User_Id");
            AddForeignKey("dbo.UserRepositoryRoles", "Repository_Id", "dbo.Repositories", "Id");
            AddForeignKey("dbo.Repositories", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
