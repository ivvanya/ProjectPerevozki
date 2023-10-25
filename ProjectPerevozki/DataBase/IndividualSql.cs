using Npgsql;
using ProjectPerevozki.Models;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ProjectPerevozki.DataBase
{
    public class IndividualSql
    {
        private SQLConnection connection;
        public IndividualSql()
        {
            connection = SQLConnection.GetInstanse();
        }

        private static IndividualSql _instanse;

        public static IndividualSql GetInstanse()
        {
            if (_instanse == null)
            {
                _instanse = new IndividualSql();
            }
            return _instanse;
        }

        public List<Individual> SelectAllIndividual(int indid = 0)
        {
            List<Individual> individualList = new List<Individual>();
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM Individual WHERE individualid!=@id", connection.Get()))
            {
                command.Parameters.AddWithValue("id", indid);
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Individual individual = new Individual
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Number = reader.GetString(2),
                        PassSeries = reader.GetString(3),
                        PassNumber = reader.GetString(4),
                        DateOfIssue = reader.GetDateTime(5),
                        IssuedBy = reader.GetString(6)
                    };
                    individualList.Add(individual);
                }
            }
            connection.Close();
            return individualList;
        }

        public void WriteToDb(Individual individual)
        {
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"INSERT INTO Individual VALUES(@indid, @name, 
                @number, @passseries, @passnumber, @date, @by)", connection.Get()))
            {
                command.Parameters.AddWithValue("indid", individual.Id);
                command.Parameters.AddWithValue("name", individual.Name);
                command.Parameters.AddWithValue("number", individual.Number);
                command.Parameters.AddWithValue("passseries", individual.PassSeries);
                command.Parameters.AddWithValue("passnumber", individual.PassNumber);
                command.Parameters.AddWithValue("date", individual.DateOfIssue);
                command.Parameters.AddWithValue("by", individual.IssuedBy);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public void DeleteFromDb(int id)
        {
            try
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("DELETE FROM Tenant WHERE tenantid=@ID", connection.Get()))
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

        public void UpdateInDb(Individual individual)
        {
            try
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(@"UPDATE Individual SET name=@name, number=@number, 
                    passseries=@passseries, passnumber=@passnumber, dateofissue=@date, issuedby=@by WHERE individualid=@id", connection.Get()))
                {
                    command.Parameters.AddWithValue("name", individual.Name);
                    command.Parameters.AddWithValue("number", individual.Number);
                    command.Parameters.AddWithValue("passseries", individual.PassSeries);
                    command.Parameters.AddWithValue("passnumber", individual.PassNumber);
                    command.Parameters.AddWithValue("date", individual.DateOfIssue);
                    command.Parameters.AddWithValue("by", individual.IssuedBy);
                    command.Parameters.AddWithValue("id", individual.Id);
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
