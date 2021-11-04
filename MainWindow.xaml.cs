using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;

namespace ComingToWork
{
    public partial class MainWindow : Window
    {
        //private EmployeeContext _employeeContext;
        private string _todayFileName;

        private bool _isEmployeeSelected;
        private string _fullNameCurrentEmployee;
        private string _getingChar;
        private WindowWithChar windowParent;

        public MainWindow(string Char, WindowWithChar windowParent)
        {
            this.windowParent = windowParent;

            InitializeComponent();
            
            //получили символ
            _getingChar = Char;
            //обнулили имя
            _fullNameCurrentEmployee = "";

            var _employeeContext = EmployeeContext.GetContext().Employees.ToList();
            var _currentEmpl = _employeeContext.Where(t=> t.LastName.ToLower().StartsWith(_getingChar.ToLower())).ToList();

            EmployeesList.ItemsSource = _currentEmpl;
        }

        private void EmployeesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeesList.SelectedItem == null)
            {
                _isEmployeeSelected = false;
                _fullNameCurrentEmployee = "";
                return;
            }
            Employee selectedEmployee = EmployeesList.SelectedItem as Employee;
            _fullNameCurrentEmployee = selectedEmployee.LastName + " " + selectedEmployee.FirstName + " " + selectedEmployee.MiddleName;
            _isEmployeeSelected = true;
        }

        private void BtnComing_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesList.SelectedItem != null)
            {
                string[] time = DateTime.Now.ToString().Split(' ');
                if (_isEmployeeSelected)
                {
                    foreach (XElement xe in WindowWithChar.RootElementXml.Elements("Employee").ToList())
                    {
                        if (xe.Element("FullName").Value == _fullNameCurrentEmployee)
                        {
                            xe.Element("TimeEntry").Value = time[1];
                        }
                    }
                }
                WindowWithChar.XDocument.Save(WindowWithChar.TodayFileName);
                windowParent.UpdateCountEmployee();

                Close();
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesList.SelectedItem != null)
            {
                string[] time = DateTime.Now.ToString().Split(' ');
                if (_isEmployeeSelected)
                {
                    foreach (XElement xe in WindowWithChar.RootElementXml.Elements("Employee").ToList())
                    {
                        if (xe.Element("FullName").Value == _fullNameCurrentEmployee)
                        {
                            xe.Element("TimeExit").Value = time[1];
                        }
                    }
                }
                WindowWithChar.XDocument.Save(WindowWithChar.TodayFileName);
                windowParent.UpdateCountEmployee();

                Close();
            }
            
        }
    }
}
