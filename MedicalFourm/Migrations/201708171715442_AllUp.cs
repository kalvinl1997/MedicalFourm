namespace MedicalFourm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllUp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Choices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsLiked = c.Boolean(),
                        IsDisliked = c.Boolean(),
                        UserId = c.Int(nullable: false),
                        QuestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.QuestionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Choices", "UserId", "dbo.Users");
            DropForeignKey("dbo.Choices", "QuestionId", "dbo.Questions");
            DropIndex("dbo.Choices", new[] { "QuestionId" });
            DropIndex("dbo.Choices", new[] { "UserId" });
            DropTable("dbo.Choices");
        }
    }
}
