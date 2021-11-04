using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComingToWork
{
    class EmployeeContext: DbContext 
    {
        private static EmployeeContext _context;

        public DbSet<Employee> Employees { get; set; }

        public EmployeeContext() : base("DefaultConnection") { }

        public static EmployeeContext GetContext()
        {
            if (_context == null)
            {
                _context = new EmployeeContext();
            }
            return _context;
        }
    }
}
