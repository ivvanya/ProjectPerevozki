using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Documents
{
    public partial class PrintContract : Form
    {
        private string cargoListText;
        public PrintContract(AccessRights globalAccessRights)
        {
            InitializeComponent();
            DocumentsSql documentsSql = new DocumentsSql();
            foreach (Contract contract in documentsSql.GetContracts())
            {
                comboBox1.Items.Add(contract.Id);
            }
            if (comboBox1.Items.Count != 0)
            {
                comboBox1.SelectedIndex = 0;
                UpdateTable();
            }
            else
            {
                MessageBox.Show("Контракты для печати не обнаружены");
                button1.Hide();
            }
                
        }

        private void UpdateTable()
        {
            DocumentsSql documentsSql = new DocumentsSql();
            Contract contract = documentsSql.GetContract(Convert.ToInt32(comboBox1.Text));
            label16.Text = contract.OrderDate.ToString("dd MMMM yyyy") + " г.";
            label17.Text = contract.Employee.Name;
            label18.Text = contract.Employee.role.Name;

            if (contract.Sender.Entity.Id == 0)
            {
                label19.Text = contract.Sender.Individual.Name;
                label20.Text = "--------";
                label21.Text = "физического лица";
            }
            else
            {
                label19.Text = contract.Sender.Entity.Name;
                label20.Text = contract.Sender.Entity.HeadName;
                label21.Text = "директора компании";
            }

            label22.Text = contract.RecipientAddress;

            if (contract.Recipient.Entity.Id == 0)
            {
                label23.Text = contract.Recipient.Individual.Name;
                label24.Text = "Номер паспорта: " + contract.Recipient.Individual.PassSeries + contract.Recipient.Individual.PassNumber;
            }
            else
            {
                label23.Text = contract.Recipient.Entity.Name;
                label24.Text = "ИНН: " + contract.Recipient.Entity.INN;
            }

            label25.Text = contract.SenderAddress;
            label26.Text = Convert.ToString(contract.Price);

            if (contract.Sender.Entity.Id == 0)
            {
                label27.Text = "--------";
                label28.Text = "--------";
                label29.Text = "--------";
                label30.Text = "--------";
            }
            else
            {
                label27.Text = contract.Sender.Entity.Address;
                label28.Text = contract.Sender.Entity.INN;
                label29.Text = contract.Sender.Entity.Bank;
                label30.Text = contract.Sender.Entity.BankAccount;
            }

            List<TripCargoList> tripCargoLists = documentsSql.GetCargoList(Convert.ToInt32(comboBox1.Text));
            int count = 0;
            cargoListText = "";

            foreach (TripCargoList tripCargoList in tripCargoLists)
            {
                cargoListText += "      1.2." + (count + 1).ToString() + ". " + tripCargoList.Name + ". Масса груза составляет "
                    + tripCargoList.Weight.ToString() + " килограмм. Страховая стоимость груза составляет " + tripCargoList.Insurance.ToString()
                    + " рублей.\r";
                count++;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var helper = new WordHelper("../../../" + "Documents/" + "dogovor.docx"); //debug
            //var helper = new WordHelper("Templates/" + "dogovor.docx"); //release
            var items = new Dictionary<string, string>
            {
                { "<Дата заключения договора>", label16.Text },
                { "<ФИО сотрудника>", label17.Text },
                { "<Должность сотрудника>", label18.Text },
                { "<Компания/физ.лицо>", label19.Text },
                { "<ФИО директора/физ.лица>", label20.Text },
                { "<Директор/физ.лицо>", label21.Text },
                { "<Перечень грузов>", cargoListText },
                { "<Адрес получателя>", label22.Text },
                { "<Компания/физ.лицо получатель>", label23.Text },
                { "<ИНН/Номер паспорта>", label24.Text },
                { "<Адрес отправителя>", label25.Text },
                { "<Стоимость перевозки>", label26.Text },
                { "<Юр. адрес>", label27.Text },
                { "<ИНН>", label28.Text },
                { "<Банк>", label29.Text },
                { "<Банковский счет>", label30.Text }
            };
            if (items["<Перечень грузов>"].Length > 255)
            {

                string fullString = items["<Перечень грузов>"];
                int cnt = 0;
                int tempNumber = 0;
                string tempKey0 = "";
                string tempKey = "<Перечень грузов" + tempNumber.ToString() + ">";
                items["<Перечень грузов>"] = fullString.Substring(0, 230) + tempKey;

                cnt += 230;
                while (cnt < fullString.Length - 1)
                {
                    tempNumber += 1;
                    tempKey0 = "<Перечень грузов" + (tempNumber - 1).ToString() + ">";
                    tempKey = "<Перечень грузов" + tempNumber.ToString() + ">";

                    if (fullString.Length - cnt < 230)
                    {
                        items.Add(tempKey0, fullString.Substring(cnt, fullString.Length - cnt));
                    }
                    else
                    {
                        items.Add(tempKey0, fullString.Substring(cnt, 230) + tempKey);
                    }
                    cnt += 230;
                }
            }
            bool createDocument = helper.Process(items);
            if (createDocument)
            {
                MessageBox.Show("Документ создан в корневой папке");
            }
            else
            {
                MessageBox.Show("Документ не создан");
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTable();
        }
    }
}
