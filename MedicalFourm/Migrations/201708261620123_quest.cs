namespace MedicalFourm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class quest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "Qdescription", c => c.String(nullable: false));
            AddColumn("dbo.Questions", "Qcatagory", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "Qcatagory");
            DropColumn("dbo.Questions", "Qdescription");
        }
    }
}
