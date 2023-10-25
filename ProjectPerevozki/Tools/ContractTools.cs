using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ProjectPerevozki.Tools
{
    public class ContractTools
    {
        private ContractSql contractSql;
        public ContractTools()
        {
            contractSql = ContractSql.GetInstanse();
        }

        public Contract addContract(Contract contract, int senderStatus, int recipientStatus)
        {
            if (senderStatus == 0)
                contract.Sender.Id = contractSql.GetIndividualId(contract.Sender.Individual.PassSeries, contract.Sender.Individual.PassNumber);
            else
                contract.Sender.Id = contractSql.GetEntitieId(contract.Sender.Entity.INN);

            if (recipientStatus == 0)
                contract.Recipient.Id = contractSql.GetIndividualId(contract.Recipient.Individual.PassSeries, contract.Recipient.Individual.PassNumber);
            else
                contract.Recipient.Id = contractSql.GetEntitieId(contract.Recipient.Entity.INN);

            string status = checkData(contract);
            if (status == "good")
            {
                return contract;
            }
            else
            {
                MessageBox.Show(status);
                return null;
            }
        }

        public Contract updateContract(Contract contract, int senderStatus, int recipientStatus)
        {
            if (senderStatus == 0)
                contract.Sender.Id = contractSql.GetIndividualId(contract.Sender.Individual.PassSeries, contract.Sender.Individual.PassNumber);
            else
                contract.Sender.Id = contractSql.GetEntitieId(contract.Sender.Entity.INN);

            if (recipientStatus == 0)
                contract.Recipient.Id = contractSql.GetIndividualId(contract.Recipient.Individual.PassSeries, contract.Recipient.Individual.PassNumber);
            else
                contract.Recipient.Id = contractSql.GetEntitieId(contract.Recipient.Entity.INN);

            string status = checkData(contract);
            if (status == "good")
            {
                contractSql.UpdateInDb(contract);
                return contract;
            }
            else
            {
                MessageBox.Show(status);
                return null;
            }
        }

        public string checkData(Contract contract)
        {
            string status;
            if (contract.RouteLength < 1 || contract.Price < 1)
                status = "Числовые значения должны быть больше 0";
            else if (contract.SenderAddress == "" || contract.RecipientAddress == "")
                status = "Заполните поля с адресами";
            else if (!CheckPass(contract.Sender.Individual.PassSeries + " " + contract.Sender.Individual.PassNumber))
                status = "Неправильный формат ввода паспорта клиента-отправителя";
            else if (!CheckPass(contract.Recipient.Individual.PassSeries + " " + contract.Recipient.Individual.PassNumber))
                status = "Неправильный формат ввода паспорта клиента-получателя";
            else if (!CheckINN(contract.Sender.Entity.INN))
                status = "Неправильный формат ввода ИНН клиента-отправителя";
            else if (!CheckINN(contract.Recipient.Entity.INN))
                status = "Неправильный формат ввода ИНН клиента-получателя";
            else if (contract.Sender.Id == 0)
                status = "В базе данных нет клиента-отправителя с таким номером документа";
            else if (contract.Recipient.Id == 0)
                status = "В базе данных нет клиента-получателя с таким номером документа";
            else
                status = "good";

            return status;
        }

        private bool CheckINN(string number)
        {
            string reg = @"^\d{10}$";
            Regex regex = new Regex(reg);
            MatchCollection match = regex.Matches(number);
            if (match.Count > 0)
            {
                return true;
            }
            return false;
        }

        private bool CheckPass(string number)
        {
            string reg = @"^\d{4} \d{6}$";
            Regex regex = new Regex(reg);
            MatchCollection match = regex.Matches(number);
            if (match.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
