namespace GithubExtension.Security.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MergeDbs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserRepositoryRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        SecurityRoleId = c.Int(nullable: false),
                        Repository_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Repositories", t => t.Repository_Id)
                .ForeignKey("dbo.SecurityRoles", t => t.SecurityRoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.SecurityRoleId)
                .Index(t => t.Repository_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.SecurityRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRepositoryRoles", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserRepositoryRoles", "SecurityRoleId", "dbo.SecurityRoles");
            DropForeignKey("dbo.UserRepositoryRoles", "Repository_Id", "dbo.Repositories");
            DropIndex("dbo.UserRepositoryRoles", new[] { "User_Id" });
            DropIndex("dbo.UserRepositoryRoles", new[] { "Repository_Id" });
            DropIndex("dbo.UserRepositoryRoles", new[] { "SecurityRoleId" });
            DropTable("dbo.SecurityRoles");
            DropTable("dbo.UserRepositoryRoles");
        }
    }
}
