using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ComingToWork
{
    class Employee
    {
        public int id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }

        public Employee(){ }

        public Employee(string firstname, string middlename, string lastname, string department) {
            FirstName = firstname;
            MiddleName = middlename;
            LastName = middlename;
            Department = department;
        }
    }
}
