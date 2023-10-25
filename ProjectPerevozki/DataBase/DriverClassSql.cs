using Npgsql;
using ProjectPerevozki.Models;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ProjectPerevozki.DataBase
{
    public class DriverClassSql
    {
        private SQLConnection connection;
        public DriverClassSql()
        {
            connection = SQLConnection.GetInstanse();
        }

        private static DriverClassSql _instanse;

        public static DriverClassSql GetInstanse()
        {
            if (_instanse == null)
            {
                _instanse = new DriverClassSql();
            }
            return _instanse;
        }

        public List<DriverClass> SelectAll(int id = 0)
        {
            List<DriverClass> driverClasses = new List<DriverClass>();
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM driverclass WHERE classid!=@id", connection.Get()))
            {
                command.Parameters.AddWithValue("id", id);
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    DriverClass driverClass = new DriverClass()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2)
                    };
                    driverClasses.Add(driverClass);
                }
            }
            connection.Close();
            return driverClasses;
        }

        public void WriteToDb(DriverClass driverClass)
        {
            if (!CheckNameExists(driverClass.Name, driverClass.Id))
            {
                MessageBox.Show("Категория с таким названием уже существует");
                return;
            }
            else
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(@"INSERT INTO driverclass(name, description) 
                VALUES(@name, @disc)", connection.Get()))
                {
                    command.Parameters.AddWithValue("name", driverClass.Name);
                    command.Parameters.AddWithValue("disc", driverClass.Description);
                    command.ExecuteNonQuery();
                }
                connection.Close();
                MessageBox.Show("Строка добавлена в базу данных");
            }
        }

        public void DeleteFromDb(int id)
        {
            try
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("DELETE FROM driverclass WHERE classid=@ID", connection.Get()))
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

        public void UpdateInDb(DriverClass driverClass)
        {
            try
            {
                if (!CheckNameExists(driverClass.Name, driverClass.Id))
                {
                    MessageBox.Show("Вид отделки с таким названием уже существует");
                    return;
                }
                else
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(@"UPDATE driverclass SET Name=@Name, Description=@Description 
                    WHERE classid=@ID", connection.Get()))
                    {
                        command.Parameters.AddWithValue("Name", driverClass.Name);
                        command.Parameters.AddWithValue("Description", driverClass.Description);
                        command.Parameters.AddWithValue("ID", driverClass.Id);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                    MessageBox.Show("Строка обновлена в базе данных");
                }
            }
            catch
            {
                MessageBox.Show("Ошибка Обновления");
            }
        }
        private bool CheckNameExists(string name, int id)
        {
            foreach (DriverClass driverClass in SelectAll(id))
            {
                if (driverClass.Name == name)
                    return false;
            }
            return true;
        }
    }
}
