using Npgsql;
using ProjectPerevozki.Models;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ProjectPerevozki.DataBase
{
    public class CargoTypeSql
    {
        private SQLConnection connection;
        public CargoTypeSql()
        {
            connection = SQLConnection.GetInstanse();
        }

        private static CargoTypeSql _instanse;

        public static CargoTypeSql GetInstanse()
        {
            if (_instanse == null)
            {
                _instanse = new CargoTypeSql();
            }
            return _instanse;
        }

        public List<CargoType> SelectAll(int id = 0)
        {
            List<CargoType> cargoTypes = new List<CargoType>();
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM cargotype WHERE cargoid!=@id", connection.Get()))
            {
                command.Parameters.AddWithValue("id", id);
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CargoType cargoType = new CargoType()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2)
                    };
                    cargoTypes.Add(cargoType);
                }
            }
            connection.Close();
            return cargoTypes;
        }

        public void WriteToDb(CargoType cargoType)
        {
            if (!CheckNameExists(cargoType.Name, cargoType.Id))
            {
                MessageBox.Show("Тип груза с таким названием уже существует");
                return;
            }
            else
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(@"INSERT INTO cargotype(name, description) 
                VALUES(@name, @disc)", connection.Get()))
                {
                    command.Parameters.AddWithValue("name", cargoType.Name);
                    command.Parameters.AddWithValue("disc", cargoType.Description);
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
                using (NpgsqlCommand command = new NpgsqlCommand("DELETE FROM cargotype WHERE cargoid=@ID", connection.Get()))
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

        public void UpdateInDb(CargoType cargoType)
        {
            try
            {
                if (!CheckNameExists(cargoType.Name, cargoType.Id))
                {
                    MessageBox.Show("Тип груза с таким названием уже существует");
                    return;
                }
                else
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(@"UPDATE cargotype SET Name=@Name, Description=@Description 
                    WHERE cargoid=@ID", connection.Get()))
                    {
                        command.Parameters.AddWithValue("Name", cargoType.Name);
                        command.Parameters.AddWithValue("Description", cargoType.Description);
                        command.Parameters.AddWithValue("ID", cargoType.Id);
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
            foreach (CargoType cargoType in SelectAll(id))
            {
                if (cargoType.Name == name)
                    return false;
            }
            return true;
        }
    }
}
