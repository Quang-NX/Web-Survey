namespace SurveyTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRequiredEmailSurvey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Surveys", "RequiredEmail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Surveys", "RequiredEmail");
        }
    }
}
