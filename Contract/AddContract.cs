using ProjectPerevozki.Models;
using ProjectPerevozki.Tools;
using System;
using System.Windows.Forms;

namespace Contract
{
    public partial class AddContract : Form
    {
        private AccessRights globalAccessRights;
        private ProjectPerevozki.Models.Contract contract;
        private bool insertOrUpdate;
        public AddContract(AccessRights globalAccessRights)
        {
            insertOrUpdate = true;
            this.globalAccessRights = globalAccessRights;
            InitializeComponent();

            comboBox1.Items.Add("Физические лица");
            comboBox1.Items.Add("Юридические лица");
            comboBox1.SelectedIndex = 0;
            comboBox2.Items.Add("Физические лица");
            comboBox2.Items.Add("Юридические лица");
            comboBox2.SelectedIndex = 0;
        }

        public AddContract(AccessRights globalAccessRights, ProjectPerevozki.Models.Contract contract)
        {
            insertOrUpdate = false;
            this.contract = contract;
            this.globalAccessRights = globalAccessRights;
            InitializeComponent();

            comboBox1.Items.Add("Физические лица");
            comboBox1.Items.Add("Юридические лица");
            comboBox1.SelectedIndex = 0;
            comboBox2.Items.Add("Физические лица");
            comboBox2.Items.Add("Юридические лица");
            comboBox2.SelectedIndex = 0;

            updateBoxes();

            if (!globalAccessRights.Edit)
            {
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                textBox6.Enabled = false;
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
            }
        }

        private void updateBoxes()
        {
            if (contract.Sender.Individual.Id == 0 && contract.Recipient.Individual.Id == 0)
            {
                comboBox1.SelectedIndex = 1;
                comboBox2.SelectedIndex = 1;
                textBox1.Text = contract.Sender.Entity.INN;
                textBox3.Text = contract.Recipient.Entity.INN;
            }
            else if (contract.Sender.Entity.Id == 0 && contract.Recipient.Entity.Id == 0)
            {
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
                textBox1.Text = contract.Sender.Individual.PassSeries + " " + contract.Sender.Individual.PassNumber;
                textBox3.Text = contract.Recipient.Individual.PassSeries + " " + contract.Recipient.Individual.PassNumber;
            }
            else if (contract.Sender.Individual.Id == 0 && contract.Recipient.Entity.Id == 0)
            {
                comboBox1.SelectedIndex = 1;
                comboBox2.SelectedIndex = 0;
                textBox1.Text = contract.Sender.Entity.INN;
                textBox3.Text = contract.Recipient.Individual.PassSeries + " " + contract.Recipient.Individual.PassNumber;
            }
            else if (contract.Sender.Entity.Id == 0 && contract.Recipient.Individual.Id == 0)
            {
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 1;
                textBox1.Text = contract.Sender.Individual.PassSeries + " " + contract.Sender.Individual.PassNumber;
                textBox3.Text = contract.Recipient.Entity.INN;
            }
            textBox2.Text = contract.SenderAddress;
            textBox4.Text = contract.RecipientAddress;
            textBox5.Text = Convert.ToString(contract.RouteLength);
            textBox6.Text = Convert.ToString(contract.Price);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
                label2.Text = "Паспортные данные\r\nФормат: 0000 000000";
            else
                label2.Text = "Номер ИНН\r\nФормат: 0000000000";
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0)
                label5.Text = "Паспортные данные\r\nФормат: 0000 000000";
            else
                label5.Text = "Номер ИНН\r\nФормат: 0000000000";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProjectPerevozki.Models.Contract contract = new ProjectPerevozki.Models.Contract();
            contract.Employee.Id = globalAccessRights.EmployeeId;
            try
            {
                contract.RouteLength = Convert.ToInt32(textBox5.Text);
                contract.Price = Convert.ToInt32(textBox6.Text);
            }
            catch
            {
                MessageBox.Show("Детали заказа заполнены некорректно");
            }
            if (comboBox1.SelectedIndex == 0)
            {
                try
                {
                    contract.Sender.Individual.PassSeries = textBox1.Text.Split(' ')[0];
                    contract.Sender.Individual.PassNumber = textBox1.Text.Split(' ')[1];
                }
                catch
                {
                    MessageBox.Show("Паспортные данные отправителя заполнены некорректно");
                    return;
                }
            }
            else
                contract.Sender.Entity.INN = textBox1.Text;

            if (comboBox2.SelectedIndex == 0)
            {
                try
                {
                    contract.Recipient.Individual.PassSeries = textBox3.Text.Split(' ')[0];
                    contract.Recipient.Individual.PassNumber = textBox3.Text.Split(' ')[1];
                }
                catch
                {
                    MessageBox.Show("Паспортные данные получателя заполнены некорректно");
                    return;
                }
            }
            else
                contract.Recipient.Entity.INN = textBox3.Text;

            contract.OrderDate = DateTime.Now;
            contract.SenderAddress = textBox2.Text;
            contract.RecipientAddress = textBox4.Text;

            ContractTools contractTools = new ContractTools();
            if (!insertOrUpdate && globalAccessRights.Edit)
            {
                contract.Id = this.contract.Id;
                contract = contractTools.updateContract(contract, comboBox1.SelectedIndex, comboBox2.SelectedIndex);
                if (contract == null)
                {
                    return;
                }
                else
                {
                    MessageBox.Show("Данные обновлены");
                    TableTrip newForm = new TableTrip(contract, globalAccessRights);
                    newForm.Show();
                    this.Close();
                }
            }
            else if (!insertOrUpdate && globalAccessRights.Read)
            {
                contract.Id = this.contract.Id;
                contract = contractTools.addContract(contract, comboBox1.SelectedIndex, comboBox2.SelectedIndex);
                if (contract == null)
                {
                    return;
                }
                else
                {
                    TableTrip newForm = new TableTrip(contract, globalAccessRights);
                    newForm.Show();
                    this.Close();
                }
            }
            else
            {
                contract = contractTools.addContract(contract, comboBox1.SelectedIndex, comboBox2.SelectedIndex);
                if (contract == null)
                {
                    return;
                }
                else
                {
                    TableTrip newForm = new TableTrip(contract, globalAccessRights);
                    newForm.Show();
                    this.Close();
                }
            }
        }
    }
}
