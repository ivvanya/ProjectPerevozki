using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using System;
using System.Windows.Forms;

namespace ReferenceBook
{
    public partial class CargoType : Form
    {
        private AccessRights globalAccessRights;
        public CargoType(AccessRights globalAccessRights)
        {
            this.globalAccessRights = globalAccessRights;
            InitializeComponent();
            UpdateTable();
        }

        private void UpdateTable()
        {
            dataGridView1.Rows.Clear();
            CargoTypeSql cargoTypeSql = new CargoTypeSql();
            foreach (ProjectPerevozki.Models.CargoType cargoType in cargoTypeSql.SelectAll())
            {
                dataGridView1.Rows.Add(cargoType.Id, cargoType.Name, cargoType.Description);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (globalAccessRights.Delete)
            {
                if (dataGridView1.CurrentRow.Cells[0].Value == null)
                    MessageBox.Show("Строка еще не добавлена в базу данных");
                else
                {
                    DialogResult result = MessageBox.Show("Вы точно хотите удалить запись?",
                        "Предупреждение", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        CargoTypeSql cargoTypeSql = new CargoTypeSql();
                        cargoTypeSql.DeleteFromDb(Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value));
                        UpdateTable();
                    }
                    else return;
                }
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
                ProjectPerevozki.Models.CargoType cargoType = new ProjectPerevozki.Models.CargoType();
                CargoTypeSql cargoTypeSql = new CargoTypeSql();
                if (dataGridView1.CurrentRow.Cells[1].Value == null)
                {
                    MessageBox.Show("Строка названия не должна быть пустой");
                    return;
                }
                else
                {
                    if (dataGridView1.CurrentRow.Cells[0].Value == null)
                    {
                        cargoType.Name = Convert.ToString(dataGridView1.CurrentRow.Cells[1].Value);
                        cargoType.Description = Convert.ToString(dataGridView1.CurrentRow.Cells[2].Value);
                        cargoTypeSql.WriteToDb(cargoType);
                    }
                    else
                    {
                        cargoType.Id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                        cargoType.Name = Convert.ToString(dataGridView1.CurrentRow.Cells[1].Value);
                        cargoType.Description = Convert.ToString(dataGridView1.CurrentRow.Cells[2].Value);
                        cargoTypeSql.UpdateInDb(cargoType);
                    }
                }
                UpdateTable();
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
                dataGridView1.Rows[i].Selected = false;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {

                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    if (dataGridView1.Rows[i].Cells[j].Value != null)
                        if (dataGridView1.Rows[i].Cells[j].Value.ToString().Contains(textBox1.Text) && textBox1.Text != "")
                        {
                            dataGridView1.Rows[i].Selected = true;
                            break;
                        }
            }
        }
    }
}
