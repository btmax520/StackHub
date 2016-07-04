namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.String(),
                        Size = c.String(),
                        Industry = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Position = c.String(),
                        Salary = c.String(),
                        Area = c.String(),
                        Experience = c.String(),
                        Education = c.String(),
                        Kind = c.String(),
                        Skill = c.String(),
                        Description = c.String(),
                        CompanyID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Companies", t => t.CompanyID, cascadeDelete: true)
                .Index(t => t.CompanyID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Phone = c.String(),
                        Email = c.String(),
                        Name = c.String(),
                        Password = c.String(),
                        Remember = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Jobs", "CompanyID", "dbo.Companies");
            DropIndex("dbo.Jobs", new[] { "CompanyID" });
            DropTable("dbo.Users");
            DropTable("dbo.Jobs");
            DropTable("dbo.Companies");
        }
    }
}
