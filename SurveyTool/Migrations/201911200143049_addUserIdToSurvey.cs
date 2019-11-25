namespace SurveyTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUserIdToSurvey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Surveys", "UserId", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "PreserveCreatedOn", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "ModifiedOn", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "DeletedOn", c => c.DateTime());
            CreateIndex("dbo.Surveys", "UserId");
            AddForeignKey("dbo.Surveys", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Surveys", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Surveys", new[] { "UserId" });
            DropColumn("dbo.AspNetUsers", "DeletedOn");
            DropColumn("dbo.AspNetUsers", "IsDeleted");
            DropColumn("dbo.AspNetUsers", "ModifiedOn");
            DropColumn("dbo.AspNetUsers", "PreserveCreatedOn");
            DropColumn("dbo.AspNetUsers", "CreatedOn");
            DropColumn("dbo.Surveys", "UserId");
        }
    }
}
