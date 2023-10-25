using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using ProjectPerevozki.Tools;
using System;
using System.Windows.Forms;

namespace Client
{
    public partial class TableEntity : Form
    {
        private AccessRights globalAccessRights;
        public TableEntity(AccessRights globalAccessRights)
        {
            this.globalAccessRights = globalAccessRights;
            InitializeComponent();
            UpdateTable();
        }

        private void UpdateTable()
        {
            dataGridView1.Rows.Clear();
            EntitySql entitySql = new EntitySql();
            foreach (Entity entity in entitySql.SelectAllEntity())
            {
                dataGridView1.Rows.Add(entity.Id, entity.Name, entity.HeadName, entity.Number,
                    entity.Address, entity.Bank, entity.BankAccount, entity.INN);
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
                    EntitySql entitySql = new EntitySql();
                    entitySql.DeleteFromDb(Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value));
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
                Entity entity = new Entity();
                entity.Id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                entity.Name = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                entity.HeadName = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                entity.Number = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                entity.Address = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                entity.Bank = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                entity.BankAccount = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                entity.INN = dataGridView1.CurrentRow.Cells[7].Value.ToString();

                EntityTools entityTools = new EntityTools();
                string status = entityTools.checkData(entity);

                if (status != "good")
                {
                    MessageBox.Show(status);
                    return;
                }
                else
                {
                    entityTools.UpdateEntity(entity);
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
            this.Hide();
        }
    }
}
