namespace Web_service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreatePhotos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        ImageData = c.Binary(),
                        IdUser = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Users",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Email = c.String(),
                    FIO = c.String(),
                    Password = c.String(),
                    Age = c.Int(nullable: false),
                    IsAdmin = c.Int(),
                })
                .PrimaryKey(t => t.Id);


        }
        
        public override void Down()
        {

            DropTable("dbo.Users");
            DropTable("dbo.Photos");
        }
    }
}
