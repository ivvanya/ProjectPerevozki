using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using System;
using System.Windows.Forms;

namespace ReferenceBook
{
    public partial class DriverClass : Form
    {
        private AccessRights globalAccessRights;
        public DriverClass(AccessRights globalAccessRights)
        {
            this.globalAccessRights = globalAccessRights;
            InitializeComponent();
            UpdateTable();
        }

        private void UpdateTable()
        {
            dataGridView1.Rows.Clear();
            DriverClassSql driverClassSql = new DriverClassSql();
            foreach (ProjectPerevozki.Models.DriverClass driverClass in driverClassSql.SelectAll())
            {
                dataGridView1.Rows.Add(driverClass.Id, driverClass.Name, driverClass.Description);
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
                        DriverClassSql driverClassSql = new DriverClassSql();
                        driverClassSql.DeleteFromDb(Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value));
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
                ProjectPerevozki.Models.DriverClass driverClass = new ProjectPerevozki.Models.DriverClass();
                DriverClassSql driverClassSql = new DriverClassSql();
                if (dataGridView1.CurrentRow.Cells[1].Value == null)
                {
                    MessageBox.Show("Строка названия не должна быть пустой");
                    return;
                }
                else
                {
                    if (dataGridView1.CurrentRow.Cells[0].Value == null)
                    {
                        driverClass.Name = Convert.ToString(dataGridView1.CurrentRow.Cells[1].Value);
                        driverClass.Description = Convert.ToString(dataGridView1.CurrentRow.Cells[2].Value);
                        driverClassSql.WriteToDb(driverClass);
                    }
                    else
                    {
                        driverClass.Id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                        driverClass.Name = Convert.ToString(dataGridView1.CurrentRow.Cells[1].Value);
                        driverClass.Description = Convert.ToString(dataGridView1.CurrentRow.Cells[2].Value);
                        driverClassSql.UpdateInDb(driverClass);
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
