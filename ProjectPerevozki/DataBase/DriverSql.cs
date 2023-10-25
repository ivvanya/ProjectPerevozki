using Npgsql;
using ProjectPerevozki.Models;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ProjectPerevozki.DataBase
{
    public class DriverSql
    {
        private SQLConnection connection;
        public DriverSql()
        {
            connection = SQLConnection.GetInstanse();
        }

        private static DriverSql _instanse;

        public static DriverSql GetInstanse()
        {
            if (_instanse == null)
            {
                _instanse = new DriverSql();
            }
            return _instanse;
        }

        public void WriteToDb(Driver driver)
        {
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"INSERT INTO Driver(name, dateofbirth, experience, classid) 
                VALUES(@name, @dateofbirth, @experience, @classid)", connection.Get()))
            {
                command.Parameters.AddWithValue("name", driver.Name);
                command.Parameters.AddWithValue("dateofbirth", driver.DateOfBirth);
                command.Parameters.AddWithValue("experience", driver.Experience);
                command.Parameters.AddWithValue("classid", driver.DriverClass.Id);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public void UpdateInDb(Driver driver)
        {
            try
            {
                DriverClass driverClass = GetDriverClass(driver.DriverClass.Name);
                driver.DriverClass.Id = driverClass.Id;
                driver.DriverClass.Name = driverClass.Name;

                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(@"UPDATE Driver SET name=@name, experience=@experience, classid=@classid, 
                    dateofbirth=@dateofbirth WHERE driverid=@driverid", connection.Get()))
                {
                    command.Parameters.AddWithValue("name", driver.Name);
                    command.Parameters.AddWithValue("experience", driver.Experience);
                    command.Parameters.AddWithValue("classid", driver.DriverClass.Id);
                    command.Parameters.AddWithValue("dateofbirth", driver.DateOfBirth);
                    command.Parameters.AddWithValue("driverid", driver.Id);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка Обновления");
            }
        }

        public void DeleteFromDb(int id)
        {
            try
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("DELETE FROM Driver WHERE driverid=@ID", connection.Get()))
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

        public List<Driver> SelectAll()
        {
            List<Driver> drivers = new List<Driver>();
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"SELECT * FROM driver d JOIN driverclass dc ON d.classid=dc.classid", connection.Get()))
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Driver driver = new Driver();
                    driver.Id = reader.GetInt32(0);
                    driver.Name = reader.GetString(1);
                    driver.Experience = reader.GetInt32(2);
                    driver.DriverClass.Id = reader.GetInt32(3);
                    driver.DriverClass.Name = reader.GetString(6);
                    driver.DateOfBirth = reader.GetDateTime(4);
                    drivers.Add(driver);
                }
            }
            connection.Close();
            return drivers;
        }

        public DriverClass GetDriverClass(string name)
        {
            DriverClass driverClass = new DriverClass();
            connection.Open();
            using (var command = new NpgsqlCommand("SELECT * FROM DriverClass WHERE Name=@Name", connection.Get()))
            {
                command.Parameters.AddWithValue("Name", name);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    driverClass.Id = reader.GetInt32(0);
                    driverClass.Name = reader.GetString(1);
                }
            }
            connection.Close();
            return driverClass;
        }
    }
}
