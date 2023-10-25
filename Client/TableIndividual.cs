using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using ProjectPerevozki.Tools;
using System;
using System.Windows.Forms;

namespace Client
{
    public partial class TableIndividual : Form
    {
        private AccessRights globalAccessRights;
        public TableIndividual(AccessRights globalAccessRights)
        {
            this.globalAccessRights = globalAccessRights;
            InitializeComponent();
            UpdateTable();
        }

        private void UpdateTable()
        {
            dataGridView1.Rows.Clear();
            IndividualSql individualSql = new IndividualSql();
            foreach (Individual individual in individualSql.SelectAllIndividual())
            {
                dataGridView1.Rows.Add(individual.Id, individual.Name, individual.Number, individual.PassSeries,
                    individual.PassNumber, individual.DateOfIssue, individual.IssuedBy);
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
                    IndividualSql individualSql = new IndividualSql();
                    individualSql.DeleteFromDb(Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value));
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
                Individual individual = new Individual();
                individual.Id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                individual.Name = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                individual.Number = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                individual.PassSeries = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                individual.PassNumber = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                individual.DateOfIssue = Convert.ToDateTime(dataGridView1.CurrentRow.Cells[5].Value);
                individual.IssuedBy = dataGridView1.CurrentRow.Cells[6].Value.ToString();

                IndividualTools individualTools = new IndividualTools();
                string status = individualTools.checkData(individual);

                if (status != "good")
                {
                    MessageBox.Show(status);
                    return;
                }
                else
                {
                    individualTools.UpdateIndividual(individual);
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
