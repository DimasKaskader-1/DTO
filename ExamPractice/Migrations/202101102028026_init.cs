namespace ExamPractice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Author",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Book",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 512),
                        AuthorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Author", t => t.AuthorId, cascadeDelete: true)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.BookReview",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReviewName = c.String(nullable: false),
                        Rating = c.Int(nullable: false),
                        BookId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Book", t => t.BookId, cascadeDelete: true)
                .Index(t => t.BookId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookReview", "BookId", "dbo.Book");
            DropForeignKey("dbo.Book", "AuthorId", "dbo.Author");
            DropIndex("dbo.BookReview", new[] { "BookId" });
            DropIndex("dbo.Book", new[] { "AuthorId" });
            DropTable("dbo.BookReview");
            DropTable("dbo.Book");
            DropTable("dbo.Author");
        }
    }
}
