namespace SurveyTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editRequiredEmailSurvey : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Surveys", "RequiredEmail", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Surveys", "RequiredEmail", c => c.Boolean());
        }
    }
}
