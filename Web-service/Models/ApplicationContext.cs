﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Web_service.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() :
        base("DefaultConnection")
        { }

        public DbSet<Order> Orders { get; set; }
    }
}