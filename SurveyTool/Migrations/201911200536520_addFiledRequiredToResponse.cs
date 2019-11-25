namespace SurveyTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFiledRequiredToResponse : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Responses", "Required", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Responses", "Required");
        }
    }
}
