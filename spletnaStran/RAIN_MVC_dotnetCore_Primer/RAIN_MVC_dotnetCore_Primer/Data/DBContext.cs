using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Vaja3.Models
{
    public class DBContext : DbContext
    {
        public DBContext (DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public DbSet<Vaja3.Models.Novica> Novica { get; set; }
        public DbSet<Vaja3.Models.Racun> Racun { get; set; }

    }
}
