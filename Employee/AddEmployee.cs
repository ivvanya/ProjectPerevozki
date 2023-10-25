using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using ProjectPerevozki.Tools;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Employee
{
    public partial class AddEmployee : Form
    {
        private AccessRights globalAccessRights;
        private List<Role> roles = new List<Role>();
        private RoleSql roleSql = new RoleSql();

        public AddEmployee(AccessRights globalAccessRights)
        {
            this.globalAccessRights = globalAccessRights;
            InitializeComponent();
            roles = roleSql.SelectAllRoles();

            foreach (Role role in roles)
                comboBox1.Items.Add(role.Name);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProjectPerevozki.Models.Employee employee = new ProjectPerevozki.Models.Employee();
            employee.Name = textBox1.Text;
            employee.Email = textBox2.Text;
            employee.PhoneNumber = textBox3.Text;
            employee.Login = textBox4.Text;
            employee.Password = textBox5.Text;
            foreach (Role role in roles)
                if (role.Name == comboBox1.Text)
                {
                    employee.RoleID = role.Id;
                    employee.role.Id = role.Id;
                    employee.role.Name = role.Name;
                    break;
                }

            EmployeeTools employeeTools = new EmployeeTools();
            string status = employeeTools.checkData(employee);

            if (status != "good")
            {
                MessageBox.Show(status);
                return;
            }
            else
            {
                employeeTools.AddEmployee(employee);
                MessageBox.Show("Пользователь добавлен");
                AddAccessRights newForm = new AddAccessRights(employee.Login);
                this.Close();
                newForm.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
