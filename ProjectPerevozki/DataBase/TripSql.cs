using Npgsql;
using ProjectPerevozki.Models;
using System.Collections.Generic;

namespace ProjectPerevozki.DataBase
{
    public class TripSql
    {
        private SQLConnection connection;
        public TripSql()
        {
            connection = SQLConnection.GetInstanse();
        }

        private static TripSql _instanse;

        public static TripSql GetInstanse()
        {
            if (_instanse == null)
            {
                _instanse = new TripSql();
            }
            return _instanse;
        }

        public int GetCargoTypeId(string name)
        {
            int id = 0;
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"SELECT * FROM CargoType WHERE name=@name", connection.Get()))
            {
                command.Parameters.AddWithValue("name", name);
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    id = reader.GetInt32(0);
                }
            }
            connection.Close();
            return id;
        }

        public int GetLastContractId()
        {
            int id;
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"SELECT contractid FROM Contract ORDER BY contractid DESC LIMIT 1",
                connection.Get()))
            {
                id = (int)command.ExecuteScalar();
            }
            connection.Close();
            return id;
        }

        public int GetLastTripId()
        {
            int id;
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"SELECT tripid FROM Trip ORDER BY tripid DESC LIMIT 1",
                connection.Get()))
            {
                id = (int)command.ExecuteScalar();
            }
            connection.Close();
            return id;
        }

        public void WriteTripToDb(Trip trip)
        {
            int contractid = GetLastContractId();
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"INSERT INTO Trip(contractid, carid, inputtime) 
                    VALUES (@contractid, @carid, @inputtime)", connection.Get()))
            {
                command.Parameters.AddWithValue("contractid", contractid);
                command.Parameters.AddWithValue("carid", trip.Car.Id);
                command.Parameters.AddWithValue("inputtime", trip.InputTime);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public void WriteCargoListToDb(List<TripCargoList> tripCargoLists)
        {
            int tripid = GetLastTripId();
            connection.Open();
            foreach (TripCargoList tripCargoList in tripCargoLists)
            {
                using (NpgsqlCommand command = new NpgsqlCommand(@"INSERT INTO TripCargoList(cargotypeid, tripid, name, amount, weight, insurance) 
                    VALUES (@cargotypeid, @tripid, @name, @amount, @weight, @insurance)", connection.Get()))
                {
                    command.Parameters.AddWithValue("cargotypeid", tripCargoList.CargoType.Id);
                    command.Parameters.AddWithValue("tripid", tripid);
                    command.Parameters.AddWithValue("name", tripCargoList.Name);
                    command.Parameters.AddWithValue("amount", tripCargoList.Amount);
                    command.Parameters.AddWithValue("weight", tripCargoList.Weight);
                    command.Parameters.AddWithValue("insurance", tripCargoList.Insurance);
                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
        }
        public void WriteDriverListToDb(List<TripDriverList> tripDriverLists)
        {
            int tripid = GetLastTripId();
            connection.Open();
            foreach (TripDriverList tripDriverList in tripDriverLists)
            {
                using (NpgsqlCommand command = new NpgsqlCommand(@"INSERT INTO TripDriverList(tripid, driverid) 
                    VALUES (@tripid, @driverid)", connection.Get()))
                {
                    command.Parameters.AddWithValue("tripid", tripid);
                    command.Parameters.AddWithValue("driverid", tripDriverList.Driver.Id);
                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
        }

        public List<Trip> SelectAllTrips(int contractid)
        {
            List<Trip> trips = new List<Trip>();
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"SELECT tripid, contractid, t.carid, c.number, c.brand, c.model, inputtime  FROM trip t JOIN car c 
                ON c.carid=t.carid WHERE contractid=@contractid", connection.Get()))
            {
                command.Parameters.AddWithValue("contractid", contractid);
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Trip trip = new Trip();
                    trip.Id = reader.GetInt32(0);
                    trip.Contract.Id = reader.GetInt32(1);
                    trip.Car.Id = reader.GetInt32(2);
                    trip.Car.Number = reader.GetString(3);
                    trip.Car.Brand = reader.GetString(4);
                    trip.Car.Model = reader.GetString(5);
                    trip.InputTime = reader.GetDateTime(6);
                    trips.Add(trip);
                }
            }
            connection.Close();
            return trips;
        }

        public List<TripCargoList> SelectAllCargoByTripId(int tripid)
        {
            List<TripCargoList> tripCargoLists = new List<TripCargoList>();
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"SELECT * FROM tripcargolist t JOIN cargotype ct ON t.cargotypeid=ct.cargoid 
                WHERE tripid=@tripid", connection.Get()))
            {
                command.Parameters.AddWithValue("tripid", tripid);
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TripCargoList tripCargoList = new TripCargoList();
                    tripCargoList.CargoType.Name = reader.GetString(8);
                    tripCargoList.Name = reader.GetString(3);
                    tripCargoList.Amount = reader.GetInt32(4);
                    tripCargoList.Weight = reader.GetInt32(5);
                    tripCargoList.Insurance = reader.GetInt32(6);
                    tripCargoLists.Add(tripCargoList);
                }
            }
            connection.Close();
            return tripCargoLists;
        }

        public int GetCarIdByTripId(int tripid)
        {
            int carid = 0;
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"SELECT carid FROM trip WHERE tripid=@tripid", connection.Get()))
            {
                command.Parameters.AddWithValue("tripid", tripid);
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    carid = reader.GetInt32(0);
                }
            }
            connection.Close();
            return carid;
        }

        public List<TripDriverList> SelectAllDriversByTripId(int tripid)
        {
            List<TripDriverList> tripDriverLists = new List<TripDriverList>();
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"SELECT * FROM tripdriverlist t JOIN driver d ON t.driverid=d.driverid 
                JOIN driverclass dc ON dc.classid=d.classid WHERE tripid=@tripid", connection.Get()))
            {
                command.Parameters.AddWithValue("tripid", tripid);
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TripDriverList tripDriverList = new TripDriverList();
                    tripDriverList.Driver.Name = reader.GetString(3);
                    tripDriverList.Driver.Experience = reader.GetInt32(4);
                    tripDriverList.Driver.DriverClass.Name = reader.GetString(8);
                    tripDriverLists.Add(tripDriverList);
                }
            }
            connection.Close();
            return tripDriverLists;
        }
    }
}
