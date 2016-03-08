namespace GithubExtension.Security.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class customclaim : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUserClaims", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.AspNetUserClaims", "Project_Id", c => c.Int());
            CreateIndex("dbo.AspNetUserClaims", "Project_Id");
            AddForeignKey("dbo.AspNetUserClaims", "Project_Id", "dbo.Projects", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserClaims", "Project_Id", "dbo.Projects");
            DropIndex("dbo.AspNetUserClaims", new[] { "Project_Id" });
            DropColumn("dbo.AspNetUserClaims", "Project_Id");
            DropColumn("dbo.AspNetUserClaims", "Discriminator");
        }
    }
}
