using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using System.Text.RegularExpressions;

namespace ProjectPerevozki.Tools
{
    public class DriverTools
    {
        private DriverSql driverSql;
        public DriverTools()
        {
            driverSql = DriverSql.GetInstanse();
        }

        public string checkData(Driver driver)
        {
            string status;
            if (driver.Experience < 0 || driver.Name == "" || driver.DriverClass.Name == null)
                status = "Поля заполнены некорректно";
            else if (!CheckPersonName(driver.Name))
                status = "Поле с именем водителя заполнено некорректно";
            else if (!CheckDriverClassExists(driver.DriverClass.Name))
                status = "Класса с таким именем не существует";
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

        private bool CheckDriverClassExists(string name)
        {
            DriverClass driverClass = driverSql.GetDriverClass(name);
            if (driverClass.Id == 0)
                return false;
            else
                return true;

        }
    }
}
