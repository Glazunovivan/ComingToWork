using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;

namespace ComingToWork
{
    public partial class WindowWithChar : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private int _countEmployee;
        public int CountEmployee
        {
            get
            {
                return _countEmployee;
            }
            set
            {
                _countEmployee = value;
                OnPropertyChanged();
            }
        }
        public string StringCountEmployee
        {
            get
            {
                return "Текущее количество сотрудников: " + _countEmployee.ToString();
            }
        }

        static public XElement RootElementXml;
        static public XDocument XDocument;
        static public string TodayFileName;

        public WindowWithChar()
        {
            DataContext = this;
            InitializeComponent();
            CheckFile();
            //ищем отмеченных сотрудников
            UpdateCountEmployee();
        }
        private void CheckFile()
        {
            string _dateToday = DateTime.Now.ToString();
            string[] temp = _dateToday.Split(' ');
            TodayFileName = temp[0] + ".xml";

            if (!File.Exists(TodayFileName))
            {
                CreateDocument(TodayFileName);
            }
            XDocument = XDocument.Load(TodayFileName);
            RootElementXml = XDocument.Element("Employees");
            //в конечном итоге файл или создан или открыт
        }

        private void CreateDocument(string fileName)
        {
            var xDoc = new XmlDocument();
            var xmlDeclaration = xDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xDoc.AppendChild(xmlDeclaration);

            var root = xDoc.CreateElement("Employees");

            foreach (var empl in EmployeeContext.GetContext().Employees.ToList())
            {
                var emplNode = xDoc.CreateElement("Employee");
                string fullName = empl.LastName + " " + empl.FirstName + " " + empl.MiddleName;

                //создаем объект для записи в xml файл
                EmployeeTemplateXML employeeTemplate = new EmployeeTemplateXML
                {
                    FullName = fullName,
                    Department = empl.Department,
                    TimeEntry = "",
                    TimeExit = ""
                };

                AddChildNode("FullName", employeeTemplate.FullName, emplNode, xDoc);
                AddChildNode("Department", employeeTemplate.Department, emplNode, xDoc);
                AddChildNode("TimeEntry", employeeTemplate.TimeEntry, emplNode, xDoc);
                AddChildNode("TimeExit", employeeTemplate.TimeExit, emplNode, xDoc);

                root.AppendChild(emplNode);
            }
            xDoc.AppendChild(root);
            xDoc.Save(fileName);
        }

        private void AddChildNode(string childName, string childText, XmlElement parentNode, XmlDocument doc)
        {
            var child = doc.CreateElement(childName);
            child.InnerText = childText;
            parentNode.AppendChild(child);
        }

        private void CounterEmployee()
        {
            CountEmployee = 0;
            foreach (XElement xe in RootElementXml.Elements("Employee").ToList())
            {
                if (xe.Element("TimeEntry").Value != "")
                {
                    CountEmployee++;
                }
                if (xe.Element("TimeExit").Value != "")
                {
                    CountEmployee--;
                }
            }
        }

        private void ButtonChar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                new MainWindow(btn.Content.ToString(), this).ShowDialog();
            }
        }

        public void UpdateCountEmployee()
        {
            CounterEmployee();
            TextBlock_CountEmployee.Text = "Текущее количество сотрудников: " + _countEmployee.ToString();
        }      
    }
}
