namespace GithubExtension.Security.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GitHubIdForRepository : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Repositories", "GitHubId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Repositories", "GitHubId");
        }
    }
}
