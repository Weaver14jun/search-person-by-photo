namespace Web_service.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class ConfigurationUser : DbMigrationsConfiguration<Web_service.Models.UserContext>
    {
        public ConfigurationUser()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Web_service.Models.UserContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
