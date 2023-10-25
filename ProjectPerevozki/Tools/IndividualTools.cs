using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using System.Text.RegularExpressions;

namespace ProjectPerevozki.Tools
{
    public class IndividualTools
    {
        private IndividualSql individualSql;
        public IndividualTools()
        {
            individualSql = IndividualSql.GetInstanse();
        }

        public void AddIndividual(Individual individual)
        {
            int id = individualSql.CreateTenantGetId(true);
            individual.Id = id;
            individualSql.WriteToDb(individual);
        }

        public void UpdateIndividual(Individual individual)
        {
            individualSql.UpdateInDb(individual);
        }

        public string checkData(Individual individual)
        {
            string status;
            if (individual.Name == "" || individual.Number == "" || individual.PassSeries == ""
                || individual.PassNumber == "" || individual.DateOfIssue.ToString() == "" || individual.IssuedBy == "")
                status = "Заполните все поля";
            else if (!CheckPersonName(individual.Name))
                status = "Имя клиента введено некорректно";
            else if (!CheckNumber(individual.Number))
                status = "Неправильный формат номера телефона";
            else if (!CheckPassNumber(individual.PassNumber))
                status = "Неправильный формат номера паспорта";
            else if (!CheckPassSeries(individual.PassSeries))
                status = "Неправильный формат серии паспорта";
            else if (!CheckPassExists(individual.PassSeries, individual.PassNumber, individual.Id))
                status = "Клиент с такими паспортными данными уже существует";
            else if (!CheckNumberExists(individual.Number, individual.Id))
                status = "Клиент с таким номером телефона уже существует";
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
        private bool CheckNumberExists(string number, int id)
        {
            foreach (Individual individual in individualSql.SelectAllIndividual(id))
            {
                if (individual.Number == number)
                    return false;
            }
            return true;
        }

        private bool CheckPassExists(string series, string number, int id)
        {
            foreach (Individual individual in individualSql.SelectAllIndividual(id))
            {
                if (individual.PassSeries == series && individual.PassNumber == number)
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

        private bool CheckPassNumber(string number)
        {
            string reg = @"^\d{6}$";
            Regex regex = new Regex(reg);
            MatchCollection match = regex.Matches(number);
            if (match.Count > 0)
            {
                return true;
            }
            return false;
        }

        private bool CheckPassSeries(string account)
        {
            string emailreg = @"^\d{4}$";
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
