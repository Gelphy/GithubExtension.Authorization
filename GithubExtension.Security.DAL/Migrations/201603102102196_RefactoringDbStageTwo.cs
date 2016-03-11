namespace GithubExtension.Security.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RefactoringDbStageTwo : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Projects", newName: "Repositories");
            DropForeignKey("dbo.AspNetUserClaims", "ProjectId", "dbo.Projects");
            DropIndex("dbo.AspNetUserClaims", new[] { "ProjectId" });
            AddColumn("dbo.Repositories", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Repositories", "User_Id");
            AddForeignKey("dbo.Repositories", "User_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.AspNetUserClaims", "ProjectId");
            DropColumn("dbo.AspNetUserClaims", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUserClaims", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.AspNetUserClaims", "ProjectId", c => c.Int());
            DropForeignKey("dbo.Repositories", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Repositories", new[] { "User_Id" });
            DropColumn("dbo.Repositories", "User_Id");
            CreateIndex("dbo.AspNetUserClaims", "ProjectId");
            AddForeignKey("dbo.AspNetUserClaims", "ProjectId", "dbo.Projects", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.Repositories", newName: "Projects");
        }
    }
}
