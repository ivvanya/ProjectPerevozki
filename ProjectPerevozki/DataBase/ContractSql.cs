using Npgsql;
using ProjectPerevozki.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ProjectPerevozki.DataBase
{
    public class ContractSql
    {
        private SQLConnection connection;
        public ContractSql()
        {
            connection = SQLConnection.GetInstanse();
        }

        private static ContractSql _instanse;

        public static ContractSql GetInstanse()
        {
            if (_instanse == null)
            {
                _instanse = new ContractSql();
            }
            return _instanse;
        }

        public List<Contract> SelectAllContracts()
        {
            List<Contract> contracts = new List<Contract>();
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"select * from contract", connection.Get()))
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Contract contract = new Contract();
                    contract.Id = reader.GetInt32(0);
                    contract.Employee.Id = reader.GetInt32(1);
                    contract.OrderDate = reader.GetDateTime(2);
                    contract.Recipient.Id = reader.GetInt32(3);
                    contract.RecipientAddress = reader.GetString(4);
                    contract.Sender.Id = reader.GetInt32(5);
                    contract.SenderAddress = reader.GetString(6);
                    contract.RouteLength = reader.GetInt32(7);
                    contract.Price = reader.GetInt32(8);
                    contracts.Add(contract);
                }
            }
            connection.Close();

            foreach (Contract contract in contracts)
            {
                contract.Sender.Individual = GetIndividual(contract.Sender.Id);
                contract.Sender.Entity = GetEntity(contract.Sender.Id);

                contract.Recipient.Individual = GetIndividual(contract.Recipient.Id);
                contract.Recipient.Entity = GetEntity(contract.Recipient.Id);
            }

            return contracts;
        }

        public void WriteToDb(Contract contract)
        {
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"INSERT INTO Contract(employeeid, orderdate, recipientid, recipientaddress, senderid, senderaddress,
                routelength, price) VALUES(@employeeid, @orderdate, @recipientid, @recipientaddress, @senderid, @senderaddress, @routelength, @price)", connection.Get()))
            {
                command.Parameters.AddWithValue("employeeid", contract.Employee.Id);
                command.Parameters.AddWithValue("orderdate", contract.OrderDate);
                command.Parameters.AddWithValue("recipientid", contract.Recipient.Id);
                command.Parameters.AddWithValue("recipientaddress", contract.RecipientAddress);
                command.Parameters.AddWithValue("senderid", contract.Sender.Id);
                command.Parameters.AddWithValue("senderaddress", contract.SenderAddress);
                command.Parameters.AddWithValue("routelength", contract.RouteLength);
                command.Parameters.AddWithValue("price", contract.Price);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public void DeleteFromDb(int id)
        {
            try
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("DELETE FROM Contract WHERE contractid=@ID", connection.Get()))
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

        public void UpdateInDb(Contract contract)
        {
            try
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(@"UPDATE Contract SET recipientid=@recipientid, recipientaddress=@recipientaddress, 
                    senderid=@senderid, senderaddress=@senderaddress, routelength=@routelength, price=@price WHERE contractid=@contractid", connection.Get()))
                {
                    command.Parameters.AddWithValue("recipientid", contract.Recipient.Id);
                    command.Parameters.AddWithValue("recipientaddress", contract.RecipientAddress);
                    command.Parameters.AddWithValue("senderid", contract.Sender.Id);
                    command.Parameters.AddWithValue("senderaddress", contract.SenderAddress);
                    command.Parameters.AddWithValue("routelength", contract.RouteLength);
                    command.Parameters.AddWithValue("price", contract.Price);
                    command.Parameters.AddWithValue("contractid", contract.Id);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка Обновления");
            }
        }

        public Individual GetIndividual(int tenantid)
        {
            Individual individual = new Individual();
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"SELECT * FROM tenant t JOIN individual i ON t.tenantid=i.individualid WHERE tenantid=@tenantid", connection.Get()))
            {
                command.Parameters.AddWithValue("tenantid", tenantid);
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    individual.Id = reader.GetInt32(2);
                    individual.Name = reader.GetString(3);
                    individual.Number = reader.GetString(4);
                    individual.PassSeries = reader.GetString(5);
                    individual.PassNumber = reader.GetString(6);
                    individual.DateOfIssue = reader.GetDateTime(7);
                    individual.IssuedBy = reader.GetString(8);
                }
            }
            connection.Close();
            return individual;
        }

        public Entity GetEntity(int tenantid)
        {
            Entity entity = new Entity();
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"SELECT * FROM tenant t JOIN entity e ON t.tenantid=e.entityid WHERE tenantid=@tenantid", connection.Get()))
            {
                command.Parameters.AddWithValue("tenantid", tenantid);
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    entity.Id = reader.GetInt32(2);
                    entity.Name = reader.GetString(3);
                    entity.HeadName = reader.GetString(4);
                    entity.Number = reader.GetString(5);
                    entity.Address = reader.GetString(6);
                    entity.Bank = reader.GetString(7);
                    entity.BankAccount = reader.GetString(8);
                    entity.INN = reader.GetString(9);
                }
            }
            connection.Close();
            return entity;
        }

        public int GetEntitieId(string number)
        {
            int id = 0;
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand("SELECT entityid FROM Entity WHERE INN=@INN", connection.Get()))
            {
                command.Parameters.AddWithValue("INN", number);
                id = Convert.ToInt32(command.ExecuteScalar());
            }
            connection.Close();
            return id;
        }

        public int GetIndividualId(string series, string number)
        {
            int id = 0;
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"SELECT individualid FROM Individual WHERE passseries=@series 
                and passnumber=@number", connection.Get()))
            {
                command.Parameters.AddWithValue("series", series);
                command.Parameters.AddWithValue("number", number);
                id = Convert.ToInt32(command.ExecuteScalar());
            }
            connection.Close();
            return id;
        }
    }
}
