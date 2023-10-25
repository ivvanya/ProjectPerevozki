using Npgsql;
using ProjectPerevozki.Models;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ProjectPerevozki.DataBase
{
    public class EntitySql
    {
        private SQLConnection connection;
        public EntitySql()
        {
            connection = SQLConnection.GetInstanse();
        }

        private static EntitySql _instanse;

        public static EntitySql GetInstanse()
        {
            if (_instanse == null)
            {
                _instanse = new EntitySql();
            }
            return _instanse;
        }

        public List<Entity> SelectAllEntity(int entid = 0)
        {
            List<Entity> entityList = new List<Entity>();
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM Entity WHERE entityid!=@id", connection.Get()))
            {
                command.Parameters.AddWithValue("id", entid);
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Entity entity = new Entity
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        HeadName = reader.GetString(2),
                        Number = reader.GetString(3),
                        Address = reader.GetString(4),
                        Bank = reader.GetString(5),
                        BankAccount = reader.GetString(6),
                        INN = reader.GetString(7)
                    };
                    entityList.Add(entity);
                }
            }
            connection.Close();
            return entityList;
        }

        public void WriteToDb(Entity entity)
        {
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"INSERT INTO Entity(entityid, name, headname, number, address, bank,
                bankaccount, inn) VALUES(@entityid, @name, @headname, @number, @address, @bank, @bankaccount, @inn)", connection.Get()))
            {
                command.Parameters.AddWithValue("entityid", entity.Id);
                command.Parameters.AddWithValue("name", entity.Name);
                command.Parameters.AddWithValue("headname", entity.HeadName);
                command.Parameters.AddWithValue("number", entity.Number);
                command.Parameters.AddWithValue("address", entity.Address);
                command.Parameters.AddWithValue("bank", entity.Bank);
                command.Parameters.AddWithValue("bankaccount", entity.BankAccount);
                command.Parameters.AddWithValue("inn", entity.INN);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public void DeleteFromDb(int id)
        {
            try
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("DELETE FROM tenant WHERE tenantid=@ID", connection.Get()))
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

        public void UpdateInDb(Entity entity)
        {
            try
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(@"UPDATE Entity SET name=@name, headname=@headname, 
                    number=@number, address=@address, bank=@bank, bankaccount=@bankaccount, inn=@inn WHERE entityid=@entityid", connection.Get()))
                {
                    command.Parameters.AddWithValue("name", entity.Name);
                    command.Parameters.AddWithValue("headname", entity.HeadName);
                    command.Parameters.AddWithValue("number", entity.Number);
                    command.Parameters.AddWithValue("address", entity.Address);
                    command.Parameters.AddWithValue("bank", entity.Bank);
                    command.Parameters.AddWithValue("bankaccount", entity.BankAccount);
                    command.Parameters.AddWithValue("inn", entity.INN);
                    command.Parameters.AddWithValue("entityid", entity.Id);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка Обновления");
            }
        }

        public int CreateTenantGetId(bool status)
        {
            int id;
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"INSERT INTO Tenant(facestatus) VALUES(@status)",
                connection.Get()))
            {
                command.Parameters.AddWithValue("status", status);
                command.ExecuteNonQuery();
            }

            using (NpgsqlCommand command = new NpgsqlCommand(@"SELECT * FROM Tenant ORDER BY tenantid DESC LIMIT 1",
                connection.Get()))
            {
                id = (int)command.ExecuteScalar();
            }
            connection.Close();
            return id;
        }
    }
}
