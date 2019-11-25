namespace SurveyTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTableCustomer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Name = c.String(),
                        Age = c.Int(),
                        Address = c.String(),
                        Gender = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Responses", "CusId", c => c.Int(nullable: false));
            CreateIndex("dbo.Responses", "CusId");
            AddForeignKey("dbo.Responses", "CusId", "dbo.Customers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Responses", "CusId", "dbo.Customers");
            DropIndex("dbo.Responses", new[] { "CusId" });
            DropColumn("dbo.Responses", "CusId");
            DropTable("dbo.Customers");
        }
    }
}
