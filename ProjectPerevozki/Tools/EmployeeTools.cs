using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ProjectPerevozki.Tools
{
    public class EmployeeTools
    {
        private EmployeeSql employeeSql;
        public EmployeeTools()
        {
            employeeSql = EmployeeSql.GetInstanse();
        }

        public Employee LoginEmployee(string login, string password)
        {
            return employeeSql.LoginEmployee(login, GetHash(password));
        }

        public void AddEmployee(Employee employee)
        {
            string password = employee.Password;
            employee.Password = GetHash(password);
            employeeSql.WriteToDb(employee);
        }

        public void UpdateEmployee(Employee employee)
        {
            employeeSql.UpdateInDb(employee);
        }

        public string checkData(Employee employee)
        {
            string status;
            if (employee.Name == "" || employee.Email == "" || employee.Password == ""
                    || employee.PhoneNumber == "" || employee.role.Name == "" || employee.Login == "")
                status = "Заполните все поля";
            else if (!CheckPersonName(employee.Name))
                status = "Имя сотрудника введено некорректно";
            else if (!CheckNumber(employee.PhoneNumber))
                status = "Неправильный формат номера телефона - номер телефона должен состоять из 11 цифр";
            else if (!CheckEmail(employee.Email))
                status = "Неправильный формат электронного адреса";
            else if (!CheckEmailExists(employee.Email, employee.Id))
                status = "Пользователь с таким электронным адресом уже существует";
            else if (!CheckLoginExists(employee.Login, employee.Id))
                status = "Пользователь с таким логином уже существует";
            else if (!CheckNumberExists(employee.PhoneNumber, employee.Id))
                status = "Пользователь с таким номером телефона уже существует";
            else if (!CkeckRoleExists(employee.role.Name))
                status = "Введите корректное название должности";
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

        private bool CkeckRoleExists(string currentRole)
        {
            RoleSql roleSql = new RoleSql();
            Role role = roleSql.GetRole(currentRole);
            if (role.Id == 0)
                return false;
            else
                return true;

        }
        public bool CheckLoginExists(string login, int id)
        {
            foreach (Employee employee in employeeSql.SelectAllEmployees(id))
            {
                if (employee.Login == login)
                    return false;
            }
            return true;
        }

        public bool CheckNumberExists(string number, int id)
        {
            foreach (Employee employee in employeeSql.SelectAllEmployees(id))
            {
                if (employee.PhoneNumber == number)
                    return false;
            }
            return true;
        }

        public bool CheckEmailExists(string email, int id)
        {
            foreach (Employee employee in employeeSql.SelectAllEmployees(id))
            {
                if (employee.Email == email)
                    return false;
            }
            return true;
        }

        public bool CheckNumber(string phoneNumber)
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

        public bool CheckEmail(string email)
        {
            string emailreg = "^[A-Za-z\\d.+]+@.+$";
            Regex regex = new Regex(emailreg);
            MatchCollection match = regex.Matches(email);
            if (match.Count > 0)
            {
                return true;
            }
            return false;
        }

        private string ByteArrayToString(byte[] input)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(input.Length);
            for (i = 0; i < input.Length; i++)
            {
                sOutput.Append(input[i].ToString("X2"));
            }
            return sOutput.ToString();
        }

        public string GetHash(string input)
        {
            byte[] tmpSource;
            byte[] tmpHash;
            tmpSource = ASCIIEncoding.ASCII.GetBytes(input);
            tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
            return ByteArrayToString(tmpHash);
        }
    }
}
