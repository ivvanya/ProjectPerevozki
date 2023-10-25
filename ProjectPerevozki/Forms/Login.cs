using ProjectPerevozki.Models;
using ProjectPerevozki.Tools;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProjectPerevozki.Forms
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();

            textBox1.Text = "Логин";
            textBox1.ForeColor = Color.Gray;
            textBox2.Text = "Пароль";
            textBox2.ForeColor = Color.Gray;

            textBox1.Enter += textBox1_Enter;
            textBox2.Enter += textBox2_Enter;
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.ForeColor != Color.Black)
            {
                textBox1.Text = null;
                textBox1.ForeColor = Color.Black;
            }
        }
        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.ForeColor != Color.Black)
            {
                textBox2.Text = null;
                textBox2.ForeColor = Color.Black;
                textBox2.PasswordChar = '*';
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;
            EmployeeTools employeeTools = new EmployeeTools();
            Employee employee = employeeTools.LoginEmployee(login, password);

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Поля заполнены некорректно");
            }
            else if (employee.Id != 0)
            {
                MainForm mainForm = new MainForm(employee);
                mainForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Сотрудник не найден. Проверьте корректность логина и пароля");
            }
        }
    }
}
