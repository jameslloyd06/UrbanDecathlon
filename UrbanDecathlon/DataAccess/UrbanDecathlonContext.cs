using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using UrbanDecathlon.Models;

namespace UrbanDecathlon.DataAccess
{
    public class UrbanDecathlonContext : DbContext
    {
        public UrbanDecathlonContext() :base()
        {

        }

        public DbSet<Template> Templates { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Athlete> Athletes { get; set; }
    }
}