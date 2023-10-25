using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using System;
using System.Windows.Forms;

namespace Documents
{
    public partial class PrintTenant : Form
    {
        private AccessRights globalAccessRights;
        public PrintTenant(AccessRights globalAccessRights)
        {
            this.globalAccessRights = globalAccessRights;
            InitializeComponent();
            UpdateTable();
            comboBox1.Items.Add("Физические лица");
            comboBox1.Items.Add("Юридические лица");
            comboBox1.SelectedIndex = 0;
        }

        private void UpdateTable()
        {
            dataGridView1.Rows.Clear();
            if (comboBox1.SelectedIndex == 0)
            {
                string[] headers = { "ФИО клиента", "Номер телефона", "Серия паспорта", "Номер паспорта", "Дата выдачи", "Кем выдан" };
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    dataGridView1.Columns[i].HeaderText = headers[i];
                }
                IndividualSql individualSql = new IndividualSql();
                foreach (Individual individual in individualSql.SelectAllIndividual())
                {
                    dataGridView1.Rows.Add(individual.Name, individual.Number, individual.PassSeries,
                        individual.PassNumber, individual.DateOfIssue.ToString("dd.MM.yyyy"), individual.IssuedBy);
                }
            }
            else
            {
                string[] headers = { "Название компании", "ФИО директора", "Номер телефона", "Юридический адрес", "Банковский счет",
                    "ИНН" };
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    dataGridView1.Columns[i].HeaderText = headers[i];
                }
                EntitySql entitySql = new EntitySql();
                foreach (Entity entity in entitySql.SelectAllEntity())
                {
                    dataGridView1.Rows.Add(entity.Name, entity.HeadName, entity.Number,
                        entity.Address, entity.Bank + ", " + entity.BankAccount, entity.INN);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                Microsoft.Office.Interop.Excel.Application xcelApp = new Microsoft.Office.Interop.Excel.Application();
                xcelApp.Application.Workbooks.Add(Type.Missing);
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    xcelApp.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
                }
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        xcelApp.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }
                xcelApp.Columns.AutoFit();
                xcelApp.Visible = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTable();
        }
    }
}
