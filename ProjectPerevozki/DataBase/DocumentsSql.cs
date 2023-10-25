using Npgsql;
using ProjectPerevozki.Models;
using System.Collections.Generic;

namespace ProjectPerevozki.DataBase
{
    public class DocumentsSql
    {
        private SQLConnection connection;
        public DocumentsSql()
        {
            connection = SQLConnection.GetInstanse();
        }

        private static DocumentsSql _instanse;

        public static DocumentsSql GetInstanse()
        {
            if (_instanse == null)
            {
                _instanse = new DocumentsSql();
            }
            return _instanse;
        }

        public List<Contract> GetContracts()
        {
            List<Contract> contracts = new List<Contract>();
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"SELECT contractid FROM contract ORDER BY contractid;", connection.Get()))
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Contract contract = new Contract();
                    contract.Id = reader.GetInt32(0);
                    contracts.Add(contract);
                }
            }
            connection.Close();
            return contracts;
        }

        public Contract GetContract(int contractid)
        {
            Contract contract = new Contract();
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"select * from contract where contractid=@contractid", connection.Get()))
            {
                command.Parameters.AddWithValue("contractid", contractid);
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    contract.Id = reader.GetInt32(0);
                    contract.Employee.Id = reader.GetInt32(1);
                    contract.OrderDate = reader.GetDateTime(2);
                    contract.Recipient.Id = reader.GetInt32(3);
                    contract.RecipientAddress = reader.GetString(4);
                    contract.Sender.Id = reader.GetInt32(5);
                    contract.SenderAddress = reader.GetString(6);
                    contract.RouteLength = reader.GetInt32(7);
                    contract.Price = reader.GetInt32(8);
                }
            }
            connection.Close();

            contract.Sender.Individual = GetIndividual(contract.Sender.Id);
            contract.Sender.Entity = GetEntity(contract.Sender.Id);

            contract.Recipient.Individual = GetIndividual(contract.Recipient.Id);
            contract.Recipient.Entity = GetEntity(contract.Recipient.Id);

            contract.Employee = GetEmployeeById(contract.Employee.Id);
            return contract;
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

        public Employee GetEmployeeById(int id)
        {
            Employee emp = new Employee();
            connection.Open();
            using (var command = new NpgsqlCommand("SELECT * FROM Employee e JOIN role r ON e.roleid=r.roleid WHERE employeeid=@employeeid", connection.Get()))
            {
                command.Parameters.AddWithValue("employeeid", id);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    emp.Id = reader.GetInt32(0);
                    emp.Name = reader.GetString(1);
                    emp.role.Name = reader.GetString(8);
                }
            }
            connection.Close();
            return emp;
        }

        public List<TripCargoList> GetCargoList(int contractid)
        {
            List<TripCargoList> tripCargoLists = new List<TripCargoList>();
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"SELECT tcl.name, tcl.weight, tcl.insurance 
                FROM tripcargolist tcl JOIN trip t ON t.tripid = tcl.tripid JOIN contract c 
                ON c.contractid = t.contractid WHERE t.contractid=@contractid", connection.Get()))
            {
                command.Parameters.AddWithValue("contractid", contractid);
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TripCargoList tripCargoList = new TripCargoList();
                    tripCargoList.Name = reader.GetString(0);
                    tripCargoList.Weight = reader.GetInt32(1);
                    tripCargoList.Insurance = reader.GetInt32(2);
                    tripCargoLists.Add(tripCargoList);
                }
            }
            connection.Close();
            return tripCargoLists;
        }
    }
}
