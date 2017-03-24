namespace _4Chan_Scraper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DbThreads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ThreadNumber = c.String(maxLength: 200),
                        DateAdded = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.ThreadNumber);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.DbThreads", new[] { "ThreadNumber" });
            DropTable("dbo.DbThreads");
        }
    }
}
