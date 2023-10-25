using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using ProjectPerevozki.Tools;
using System;
using System.Windows.Forms;

namespace Other
{
    public partial class ChangePersonalData : Form
    {
        private AccessRights globalAccessRights;
        public ChangePersonalData(AccessRights globalAccessRights)
        {
            this.globalAccessRights = globalAccessRights;
            InitializeComponent();
            UpdateTextBoxes();
        }

        private void UpdateTextBoxes()
        {
            EmployeeSql employeeSql = new EmployeeSql();
            Employee employee = employeeSql.GetEmployeeById(globalAccessRights.EmployeeId);
            textBox2.Text = employee.Email;
            textBox3.Text = employee.PhoneNumber;
            textBox4.Text = employee.Login;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Employee employee = new Employee
            {
                Id = globalAccessRights.EmployeeId,
                Email = textBox2.Text,
                PhoneNumber = textBox3.Text
            };

            string status = checkPersonalData(employee);
            if (status != "good")
            {
                MessageBox.Show(status);
                return;
            }
            else
            {
                EmployeeSql employeeSql = new EmployeeSql();
                employeeSql.UpdatePersonalData(employee);
                MessageBox.Show("Персональные данные обновлены");
                UpdateTextBoxes();
            }
        }

        public string checkPersonalData(Employee employee)
        {
            string status;
            EmployeeTools employeeTools = new EmployeeTools();
            if (employee.Email == "" || employee.PhoneNumber == "")
                status = "Заполните все поля";
            else if (!employeeTools.CheckNumber(employee.PhoneNumber))
                status = "Неправильный формат номера телефона";
            else if (!employeeTools.CheckEmail(employee.Email))
                status = "Неправильный формат электронного адреса";
            else if (!employeeTools.CheckEmailExists(employee.Email, employee.Id))
                status = "Пользователь с таким электронным адресом уже существует";
            else if (!employeeTools.CheckNumberExists(employee.PhoneNumber, employee.Id))
                status = "Пользователь с таким номером телефона уже существует";
            else
                status = "good";

            return status;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Employee employee = new Employee
            {
                Id = globalAccessRights.EmployeeId,
                Login = textBox4.Text,
                Password = textBox5.Text
            };

            string status = checkAuthData(employee);
            if (status != "good")
            {
                MessageBox.Show(status);
                return;
            }
            else
            {
                EmployeeTools employeeTools = new EmployeeTools();
                employee.Password = employeeTools.GetHash(employee.Password);
                EmployeeSql employeeSql = new EmployeeSql();
                employeeSql.UpdateAuthData(employee);
                MessageBox.Show("Данные для авторизации обновлены");
                UpdateTextBoxes();
            }
        }

        public string checkAuthData(Employee employee)
        {
            string status;
            EmployeeTools employeeTools = new EmployeeTools();
            if (employee.Login == "" || employee.Password == "")
                status = "Заполните все поля";
            else if (!employeeTools.CheckLoginExists(employee.PhoneNumber, employee.Id))
                status = "Пользователь с таким логином уже существует";
            else
                status = "good";

            return status;
        }
    }
}
