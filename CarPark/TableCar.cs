using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using System;
using System.Windows.Forms;

namespace CarPark
{
    public partial class TableCar : Form
    {
        private AccessRights globalAccessRights;
        public TableCar(AccessRights globalAccessRights)
        {
            this.globalAccessRights = globalAccessRights;
            InitializeComponent();
            UpdateTable();
        }

        private void UpdateTable()
        {
            dataGridView1.Rows.Clear();
            CarSql carSql = new CarSql();
            foreach (Car car in carSql.SelectAll())
            {
                dataGridView1.Rows.Add(car.Id, car.Number, car.Brand, car.Model, car.CapLoad);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (globalAccessRights.Write)
            {
                AddCar newForm = new AddCar();
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (globalAccessRights.Delete)
            {
                DialogResult result = MessageBox.Show("Вы точно хотите удалить запись?",
                "Предупреждение", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    CarSql carSql = new CarSql();
                    carSql.DeleteFromDb(Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value));
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
            if (globalAccessRights.Edit || globalAccessRights.Read)
            {
                CarSql carSql = new CarSql();
                Car car = carSql.GetCar(Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value));
                AddCar newForm = new AddCar(car, globalAccessRights);
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

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (globalAccessRights.Edit || globalAccessRights.Read)
            {
                Car car = new Car()
                {
                    Id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value)
                };

                AddCargo newForm = new AddCargo(car, false, globalAccessRights);
                newForm.Show();
            }
            else
            {
                MessageBox.Show("У вас недостаточно прав");
                return;
            }
        }
    }
}
