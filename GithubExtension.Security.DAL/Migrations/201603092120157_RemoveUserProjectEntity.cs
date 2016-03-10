namespace GithubExtension.Security.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveUserProjectEntity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserProjects", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.UserProjectRoles", "UserProject_Id", "dbo.UserProjects");
            DropForeignKey("dbo.UserProjectRoles", "Role_Id", "dbo.AspNetRoles");
            DropForeignKey("dbo.UserProjects", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "Project_Id", "dbo.Projects");
            DropIndex("dbo.UserProjects", new[] { "ProjectId" });
            DropIndex("dbo.UserProjects", new[] { "User_Id" });
            DropIndex("dbo.UserProjectRoles", new[] { "UserProject_Id" });
            DropIndex("dbo.UserProjectRoles", new[] { "Role_Id" });
            RenameColumn(table: "dbo.AspNetUserClaims", name: "Project_Id", newName: "ProjectId");
            RenameIndex(table: "dbo.AspNetUserClaims", name: "IX_Project_Id", newName: "IX_ProjectId");
            AddColumn("dbo.AspNetUsers", "ProviderId", c => c.Int(nullable: false));
            AddForeignKey("dbo.AspNetUserClaims", "ProjectId", "dbo.Projects", "Id", cascadeDelete: true);
            DropColumn("dbo.AspNetRoles", "Discriminator");
            DropTable("dbo.UserProjects");
            DropTable("dbo.UserProjectRoles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserProjectRoles",
                c => new
                    {
                        UserProject_Id = c.Int(nullable: false),
                        Role_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserProject_Id, t.Role_Id });
            
            CreateTable(
                "dbo.UserProjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        UserId = c.Guid(nullable: false),
                        Description = c.String(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.AspNetUserClaims", "ProjectId", "dbo.Projects");
            DropColumn("dbo.AspNetUsers", "ProviderId");
            RenameIndex(table: "dbo.AspNetUserClaims", name: "IX_ProjectId", newName: "IX_Project_Id");
            RenameColumn(table: "dbo.AspNetUserClaims", name: "ProjectId", newName: "Project_Id");
            CreateIndex("dbo.UserProjectRoles", "Role_Id");
            CreateIndex("dbo.UserProjectRoles", "UserProject_Id");
            CreateIndex("dbo.UserProjects", "User_Id");
            CreateIndex("dbo.UserProjects", "ProjectId");
            AddForeignKey("dbo.AspNetUserClaims", "Project_Id", "dbo.Projects", "Id");
            AddForeignKey("dbo.UserProjects", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.UserProjectRoles", "Role_Id", "dbo.AspNetRoles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserProjectRoles", "UserProject_Id", "dbo.UserProjects", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserProjects", "ProjectId", "dbo.Projects", "Id", cascadeDelete: true);
        }
    }
}
