using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Contract
{
    public partial class TableContract : Form
    {
        private AccessRights globalAccessRights;
        private List<ProjectPerevozki.Models.Contract> contracts;
        public TableContract(AccessRights globalAccessRights)
        {
            ContractSql contractSql = new ContractSql();
            contracts = contractSql.SelectAllContracts();
            this.globalAccessRights = globalAccessRights;

            InitializeComponent();
            UpdateTable();
        }

        private void UpdateTable()
        {
            dataGridView1.Rows.Clear();
            ContractSql contractSql = new ContractSql();
            foreach (ProjectPerevozki.Models.Contract contract in contractSql.SelectAllContracts())
            {
                if (contract.Sender.Individual.Id == 0 && contract.Recipient.Individual.Id == 0)
                {
                    dataGridView1.Rows.Add(contract.Id, contract.OrderDate, contract.Sender.Entity.Name, contract.SenderAddress,
                        contract.Recipient.Entity.Name, contract.RecipientAddress, contract.RouteLength, contract.Price);
                }
                else if (contract.Sender.Entity.Id == 0 && contract.Recipient.Entity.Id == 0)
                {
                    dataGridView1.Rows.Add(contract.Id, contract.OrderDate, contract.Sender.Individual.Name, contract.SenderAddress,
                        contract.Recipient.Individual.Name, contract.RecipientAddress, contract.RouteLength, contract.Price);
                }
                else if (contract.Sender.Individual.Id == 0 && contract.Recipient.Entity.Id == 0)
                {
                    dataGridView1.Rows.Add(contract.Id, contract.OrderDate, contract.Sender.Entity.Name, contract.SenderAddress,
                        contract.Recipient.Individual.Name, contract.RecipientAddress, contract.RouteLength, contract.Price);
                }
                else if (contract.Sender.Entity.Id == 0 && contract.Recipient.Individual.Id == 0)
                {
                    dataGridView1.Rows.Add(contract.Id, contract.OrderDate, contract.Sender.Individual.Name, contract.SenderAddress,
                        contract.Recipient.Entity.Name, contract.RecipientAddress, contract.RouteLength, contract.Price);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProjectPerevozki.Models.Contract currentContract = new ProjectPerevozki.Models.Contract();
            foreach (ProjectPerevozki.Models.Contract contract in contracts)
                if (contract.Id == Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value))
                {
                    currentContract = contract;
                    break;
                }
            if (currentContract.Id == 0)
            {
                MessageBox.Show("Выберите или создайте контракт");
                return;
            }

            if (globalAccessRights.Edit)
            {
                AddContract addContract = new AddContract(globalAccessRights, currentContract);
                if (addContract.ShowDialog() == DialogResult.OK)
                {
                    UpdateTable();
                    return;
                }
            }
            else if (globalAccessRights.Read)
            {
                AddContract addContract = new AddContract(globalAccessRights, currentContract);
                if (addContract.ShowDialog() == DialogResult.OK)
                {
                    UpdateTable();
                    return;
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
                    ContractSql contractSql = new ContractSql();
                    contractSql.DeleteFromDb(Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value));
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
            this.Close();
        }
    }
}
