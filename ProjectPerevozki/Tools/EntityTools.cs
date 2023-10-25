using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using System.Text.RegularExpressions;

namespace ProjectPerevozki.Tools
{
    public class EntityTools
    {
        private EntitySql entitySql;
        public EntityTools()
        {
            entitySql = EntitySql.GetInstanse();
        }

        public void AddEntity(Entity entity)
        {
            int id = entitySql.CreateTenantGetId(false);
            entity.Id = id;
            entitySql.WriteToDb(entity);
        }

        public void UpdateEntity(Entity entity)
        {
            entitySql.UpdateInDb(entity);
        }

        public string checkData(Entity entity)
        {
            string status;
            if (entity.Name == "" || entity.HeadName == "" || entity.Number == ""
                    || entity.Address == "" || entity.Bank == "" || entity.BankAccount == "" || entity.INN == "")
                status = "Заполните все поля";
            else if (!CheckCompanyName(entity.Name))
                status = "Название компании введено некорректно";
            else if (!CheckPersonName(entity.HeadName))
                status = "Имя директора компании введено некорректно";
            else if (!CheckNumber(entity.Number))
                status = "Неправильный формат номера телефона - номер телефона должен состоять из 11 цифр";
            else if (!CheckBankAccount(entity.BankAccount))
                status = "Неправильный формат банковского счета - банковский счет должен состоять из 20 цифр";
            else if (!CheckInn(entity.INN))
                status = "Неправильный формат - ИНН должен состоять из 20 цифр";
            else if (!CheckInnExists(entity.INN, entity.Id))
                status = "Клиент с таким ИНН уже существует";
            else if (!CkeckBankAccountExists(entity.BankAccount, entity.Id))
                status = "Клиент с таким банковским счетом уже существует";
            else if (!CheckNumberExists(entity.Number, entity.Id))
                status = "Клиент с таким номером телефона уже существует";
            else if (!CkeckNameExists(entity.Name, entity.Id))
                status = "Клиент с таким названием уже существует";
            else
                status = "good";

            return status;
        }

        private bool CheckPersonName(string name)
        {
            string pattern = "^([а-яa-z])+ ([а-яa-z])+( )?([а-яa-z])*$";
            if (Regex.IsMatch(name, pattern, RegexOptions.IgnoreCase))
                return true;
            return false;
        }

        private bool CheckCompanyName(string name)
        {
            string pattern = "^(([а-яa-z])+ ?)*$";
            if (Regex.IsMatch(name, pattern, RegexOptions.IgnoreCase))
                return true;
            return false;
        }

        private bool CkeckBankAccountExists(string account, int id)
        {
            foreach (Entity entity in entitySql.SelectAllEntity(id))
            {
                if (entity.BankAccount == account)
                    return false;
            }
            return true;
        }

        private bool CkeckNameExists(string name, int id)
        {
            foreach (Entity entity in entitySql.SelectAllEntity(id))
            {
                if (entity.Name == name)
                    return false;
            }
            return true;
        }

        private bool CheckNumberExists(string number, int id)
        {
            foreach (Entity entity in entitySql.SelectAllEntity(id))
            {
                if (entity.Number == number)
                    return false;
            }
            return true;
        }

        private bool CheckInnExists(string inn, int id)
        {
            foreach (Entity entity in entitySql.SelectAllEntity(id))
            {
                if (entity.INN == inn)
                    return false;
            }
            return true;
        }

        private bool CheckNumber(string phoneNumber)
        {
            string reg = @"^\d{11}$";
            Regex regex = new Regex(reg);
            MatchCollection match = regex.Matches(phoneNumber);
            if (match.Count > 0)
            {
                return true;
            }
            return false;
        }

        private bool CheckInn(string inn)
        {
            string reg = @"^\d{10}$";
            Regex regex = new Regex(reg);
            MatchCollection match = regex.Matches(inn);
            if (match.Count > 0)
            {
                return true;
            }
            return false;
        }

        private bool CheckBankAccount(string account)
        {
            string emailreg = @"^\d{20}$";
            Regex regex = new Regex(emailreg);
            MatchCollection match = regex.Matches(account);
            if (match.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
