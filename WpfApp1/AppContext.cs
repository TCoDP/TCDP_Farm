using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class AppContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public AppContext() : base("DefaultConnection") { }
    }
}
