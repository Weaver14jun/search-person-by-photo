using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Web_service.Models
{
    public class PhotosContext : DbContext
    {
        public PhotosContext() :
        base("DefaultConnection")
        { }

        public DbSet<Photos> Photos { get; set; }
    }
}