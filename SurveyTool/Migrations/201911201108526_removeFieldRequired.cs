namespace SurveyTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeFieldRequired : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "Required", c => c.Boolean(nullable: false));
            DropColumn("dbo.Responses", "Required");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Responses", "Required", c => c.Boolean(nullable: false));
            DropColumn("dbo.Questions", "Required");
        }
    }
}
