using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using ProjectPerevozki.Tools;
using System;
using System.Windows.Forms;

namespace CarPark
{
    public partial class TableDriver : Form
    {
        private AccessRights globalAccessRights;
        public TableDriver(AccessRights globalAccessRights)
        {
            this.globalAccessRights = globalAccessRights;
            InitializeComponent();
            UpdateTable();
        }

        private void UpdateClassColumn()
        {
            DriverClassSql driverClassSql = new DriverClassSql();
            foreach (DriverClass driverClass in driverClassSql.SelectAll())
            {
                Column5.Items.Add(driverClass.Name);
            }
        }

        private void UpdateTable()
        {
            UpdateClassColumn();

            dataGridView1.Rows.Clear();
            DriverSql driverSql = new DriverSql();
            foreach (Driver driver in driverSql.SelectAll())
            {
                dataGridView1.Rows.Add(driver.Id, driver.Name, driver.DateOfBirth.ToString("dd.MM.yyyy"), driver.Experience, driver.DriverClass.Name);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (globalAccessRights.Delete)
            {
                DialogResult result = MessageBox.Show("Вы точно хотите удалить запись?",
                "Предупреждение", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    DriverSql driverSql = new DriverSql();
                    driverSql.DeleteFromDb(Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value));
                    UpdateTable();
                }
                else return;
            }
            else
            {
                MessageBox.Show("У вас недостаточно прав");
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (globalAccessRights.Edit)
            {
                Driver driver = new Driver();
                driver.Id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                driver.Name = Convert.ToString(dataGridView1.CurrentRow.Cells[1].Value);
                driver.DriverClass.Name = Convert.ToString(dataGridView1.CurrentRow.Cells[4].Value);
                driver.DateOfBirth = Convert.ToDateTime(dataGridView1.CurrentRow.Cells[2].Value);
                try
                {
                    driver.Experience = Convert.ToInt32(dataGridView1.CurrentRow.Cells[3].Value);
                }
                catch
                {
                    MessageBox.Show("Неправильный формат ввода - символы в числовых полях");
                    return;
                }

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
                    driverSql.UpdateInDb(driver);
                    MessageBox.Show("Данные пользователя обновлены");
                    UpdateTable();
                }
            }
            else
            {
                MessageBox.Show("У вас недостаточно прав");
                return;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (globalAccessRights.Write)
            {
                AddDriver newForm = new AddDriver();
                if (newForm.ShowDialog() == DialogResult.OK)
                {
                    UpdateTable();
                }
            }
            else
            {
                MessageBox.Show("У вас недостаточно прав");
                return;
            }

        }
    }
}
