using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using ProjectPerevozki.Tools;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CarPark
{
    public partial class AddDriver : Form
    {
        private List<DriverClass> driverClasses = new List<DriverClass>();
        public AddDriver()
        {
            TopMost = true;
            InitializeComponent();
            UpdateDriverClassBox();
        }

        private void UpdateDriverClassBox()
        {
            comboBox1.Items.Clear();
            DriverClassSql driverClassSql = new DriverClassSql();
            foreach (DriverClass driverClass in driverClassSql.SelectAll())
            {
                driverClasses.Add(driverClass);
                comboBox1.Items.Add(driverClass.Name);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Driver driver = new Driver();
            foreach (DriverClass driverClass in driverClasses)
            {
                if (comboBox1.Text == driverClass.Name)
                {
                    driver.DriverClass.Id = driverClass.Id;
                    driver.DriverClass.Name = driverClass.Name;
                    break;
                }
            }
            try
            {
                driver.Experience = Convert.ToInt32(textBox2.Text);
            }
            catch
            {
                MessageBox.Show("Неправильный формат ввода стажа");
                return;
            }
            driver.Name = textBox1.Text;
            driver.DateOfBirth = Convert.ToDateTime(dateTimePicker1.Text);

            DriverTools driverTools = new DriverTools();
            string status = driverTools.checkData(driver);

            if (status != "good")
            {
                MessageBox.Show(status);
                return;
            }
            else
            {
                DriverSql driverSql = new DriverSql();
                driverSql.WriteToDb(driver);
                MessageBox.Show("Данные водителя добавлены");
                this.Close();
                DialogResult = DialogResult.OK;
            }
        }
    }
}
