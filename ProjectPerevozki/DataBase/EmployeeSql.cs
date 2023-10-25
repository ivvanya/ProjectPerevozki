using Npgsql;
using ProjectPerevozki.Models;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ProjectPerevozki.DataBase
{
    public class EmployeeSql
    {
        private SQLConnection connection;
        public EmployeeSql()
        {
            connection = SQLConnection.GetInstanse();
        }

        private static EmployeeSql _instanse;

        public static EmployeeSql GetInstanse()
        {
            if (_instanse == null)
            {
                _instanse = new EmployeeSql();
            }
            return _instanse;
        }

        public List<Employee> EmployeeWithRoles()
        {
            List<Employee> employees = new List<Employee>();
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"SELECT employeeid, employee.name, email, number, role.roleid, role.name, 
                login, password FROM employee JOIN role ON employee.roleid=role.roleid ORDER BY employeeid;", connection.Get()))
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Employee emp = new Employee();
                    emp.Id = reader.GetInt32(0);
                    emp.Name = reader.GetString(1);
                    emp.Email = reader.GetString(2);
                    emp.PhoneNumber = reader.GetString(3);
                    emp.RoleID = reader.GetInt32(4);
                    emp.role.Id = reader.GetInt32(4);
                    emp.role.Name = reader.GetString(5);
                    emp.Login = reader.GetString(6);
                    emp.Password = reader.GetString(7);
                    employees.Add(emp);
                }
            }
            connection.Close();
            return employees;
        }

        public List<Employee> SelectAllEmployees(int empid = 0)
        {
            List<Employee> employees = new List<Employee>();
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM Employee WHERE employeeid!=@id", connection.Get()))
            {
                command.Parameters.AddWithValue("id", empid);
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Employee emp = new Employee
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Email = reader.GetString(2),
                        PhoneNumber = reader.GetString(3),
                        RoleID = reader.GetInt32(4),
                        Login = reader.GetString(5),
                        Password = reader.GetString(6)
                    };
                    employees.Add(emp);
                }
            }
            connection.Close();
            return employees;
        }

        public void WriteToDb(Employee employee)
        {
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"INSERT INTO Employee(Name, Email, Number, RoleID, Login, Password) 
                VALUES(@Name, @Email, @Number, @RoleID, @Login, @Password)", connection.Get()))
            {
                command.Parameters.AddWithValue("Name", employee.Name);
                command.Parameters.AddWithValue("Email", employee.Email);
                command.Parameters.AddWithValue("Number", employee.PhoneNumber);
                command.Parameters.AddWithValue("RoleID", employee.RoleID);
                command.Parameters.AddWithValue("Login", employee.Login);
                command.Parameters.AddWithValue("Password", employee.Password);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public void DeleteFromDb(int id)
        {
            try
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("DELETE FROM Employee WHERE EmployeeID=@ID", connection.Get()))
                {
                    command.Parameters.AddWithValue("ID", id);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка удаления");
            }
        }

        public void UpdateInDb(Employee employee)
        {
            try
            {
                RoleSql roleSql = new RoleSql();
                Role role = new Role();
                role = roleSql.GetRole(employee.role.Name);
                employee.role.Id = role.Id;
                employee.role.Name = role.Name;

                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(@"UPDATE Employee SET Name=@Name, Email=@Email, 
                    Number=@Number, RoleID=@RoleID, Login=@Login WHERE EmployeeID=@EmployeeID", connection.Get()))
                {
                    command.Parameters.AddWithValue("Name", employee.Name);
                    command.Parameters.AddWithValue("Email", employee.Email);
                    command.Parameters.AddWithValue("Number", employee.PhoneNumber);
                    command.Parameters.AddWithValue("RoleID", employee.role.Id);
                    command.Parameters.AddWithValue("Login", employee.Login);
                    command.Parameters.AddWithValue("EmployeeID", employee.Id);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка Обновления");
            }
        }

        public Employee GetEmployeeByLog(string login)
        {
            Employee emp = new Employee();
            connection.Open();
            using (var command = new NpgsqlCommand("SELECT * FROM Employee WHERE Login=@login", connection.Get()))
            {
                command.Parameters.AddWithValue("Login", login);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    emp.Id = reader.GetInt32(0);
                    emp.Name = reader.GetString(1);
                    emp.Email = reader.GetString(2);
                    emp.PhoneNumber = reader.GetString(3);
                    emp.RoleID = reader.GetInt32(4);
                    emp.Login = reader.GetString(5);
                    emp.Password = reader.GetString(6);
                }
            }
            connection.Close();
            return emp;
        }

        public Employee GetEmployeeById(int id)
        {
            Employee emp = new Employee();
            connection.Open();
            using (var command = new NpgsqlCommand("SELECT * FROM Employee WHERE employeeid=@employeeid", connection.Get()))
            {
                command.Parameters.AddWithValue("employeeid", id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    emp.Id = reader.GetInt32(0);
                    emp.Name = reader.GetString(1);
                    emp.Email = reader.GetString(2);
                    emp.PhoneNumber = reader.GetString(3);
                    emp.RoleID = reader.GetInt32(4);
                    emp.Login = reader.GetString(5);
                    emp.Password = reader.GetString(6);
                }
            }
            connection.Close();
            return emp;
        }

        public Employee LoginEmployee(string login, string password)
        {
            Employee employee = new Employee();
            connection.Open();
            using (var command = new NpgsqlCommand(@"SELECT * FROM Employee e JOIN Role r ON e.roleid=r.roleid
                WHERE login=@login and password=@password", connection.Get()))
            {
                command.Parameters.AddWithValue("login", login);
                command.Parameters.AddWithValue("password", password);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    employee.Id = reader.GetInt32(0);
                    employee.Name = reader.GetString(1);
                    employee.Email = reader.GetString(2);
                    employee.PhoneNumber = reader.GetString(3);
                    employee.role.Id = reader.GetInt32(4);
                    employee.Login = reader.GetString(5);
                    employee.Password = reader.GetString(6);
                    employee.role.Name = reader.GetString(8);
                }
            }
            connection.Close();
            return employee;
        }

        public void UpdatePersonalData(Employee employee)
        {
            try
            {
                RoleSql roleSql = new RoleSql();
                Role role = new Role();
                role = roleSql.GetRole(employee.role.Name);
                employee.role.Id = role.Id;
                employee.role.Name = role.Name;

                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(@"UPDATE Employee SET Email=@Email, 
                    Number=@Number WHERE EmployeeID=@EmployeeID", connection.Get()))
                {
                    command.Parameters.AddWithValue("Email", employee.Email);
                    command.Parameters.AddWithValue("Number", employee.PhoneNumber);
                    command.Parameters.AddWithValue("EmployeeID", employee.Id);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка Обновления");
            }
        }

        public void UpdateAuthData(Employee employee)
        {
            try
            {
                RoleSql roleSql = new RoleSql();
                Role role = new Role();
                role = roleSql.GetRole(employee.role.Name);
                employee.role.Id = role.Id;
                employee.role.Name = role.Name;

                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(@"UPDATE Employee SET login=@login, 
                    password=@password WHERE EmployeeID=@EmployeeID", connection.Get()))
                {
                    command.Parameters.AddWithValue("login", employee.Login);
                    command.Parameters.AddWithValue("password", employee.Password);
                    command.Parameters.AddWithValue("EmployeeID", employee.Id);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка Обновления");
            }
        }
    }
}
