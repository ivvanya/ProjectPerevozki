using Npgsql;
using ProjectPerevozki.Models;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ProjectPerevozki.DataBase
{
    public class CarSql
    {
        private SQLConnection connection;
        public CarSql()
        {
            connection = SQLConnection.GetInstanse();
        }

        private static CarSql _instanse;

        public static CarSql GetInstanse()
        {
            if (_instanse == null)
            {
                _instanse = new CarSql();
            }
            return _instanse;
        }

        public void WriteToDb(Car car)
        {
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"INSERT INTO Car(number, brand, model, capload, 
                dateofissue, dateofto, mileage, picture) VALUES(@number, @brand, @model, @capload, @dateofissue, 
                @dateofto, @mileage, @picture)", connection.Get()))
            {
                command.Parameters.AddWithValue("number", car.Number);
                command.Parameters.AddWithValue("brand", car.Brand);
                command.Parameters.AddWithValue("model", car.Model);
                command.Parameters.AddWithValue("capload", car.CapLoad);
                command.Parameters.AddWithValue("dateofissue", car.DateOfIssue);
                command.Parameters.AddWithValue("dateofto", car.DateOfTo);
                command.Parameters.AddWithValue("mileage", car.Mileage);
                command.Parameters.AddWithValue("picture", car.Picture);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public void UpdateInDb(Car car)
        {
            try
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(@"UPDATE Car SET number=@number, brand=@brand, model=@model, 
                    capload=@capload, dateofissue=@dateofissue, dateofto=@dateofto, mileage=@mileage, 
                    picture=@picture WHERE carid=@carid;", connection.Get()))
                {
                    command.Parameters.AddWithValue("number", car.Number);
                    command.Parameters.AddWithValue("brand", car.Brand);
                    command.Parameters.AddWithValue("model", car.Model);
                    command.Parameters.AddWithValue("capload", car.CapLoad);
                    command.Parameters.AddWithValue("dateofissue", car.DateOfIssue);
                    command.Parameters.AddWithValue("dateofto", car.DateOfTo);
                    command.Parameters.AddWithValue("mileage", car.Mileage);
                    command.Parameters.AddWithValue("picture", car.Picture);
                    command.Parameters.AddWithValue("carid", car.Id);
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
                using (NpgsqlCommand command = new NpgsqlCommand("DELETE FROM Car WHERE carid=@ID", connection.Get()))
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

        public List<Car> SelectAll(int carid = 0)
        {
            List<Car> cars = new List<Car>();
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"SELECT * FROM car  WHERE carid!=@id", connection.Get()))
            {
                command.Parameters.AddWithValue("id", carid);
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Car car = new Car();
                    car.Id = reader.GetInt32(0);
                    car.Number = reader.GetString(1);
                    car.Brand = reader.GetString(2);
                    car.Model = reader.GetString(3);
                    car.CapLoad = reader.GetInt32(4);
                    car.DateOfIssue = reader.GetDateTime(5);
                    car.DateOfTo = reader.GetDateTime(6);
                    car.Mileage = reader.GetInt32(7);
                    car.Picture = reader.GetFieldValue<byte[]>(8);
                    cars.Add(car);
                }
            }
            connection.Close();
            return cars;
        }

        public Car GetCar(int carid)
        {
            Car car = new Car();
            connection.Open();
            using (var command = new NpgsqlCommand(@"SELECT * FROM car  WHERE carid=@carid", connection.Get()))
            {
                command.Parameters.AddWithValue("carid", carid);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    car.Id = reader.GetInt32(0);
                    car.Number = reader.GetString(1);
                    car.Brand = reader.GetString(2);
                    car.Model = reader.GetString(3);
                    car.CapLoad = reader.GetInt32(4);
                    car.DateOfIssue = reader.GetDateTime(5);
                    car.DateOfTo = reader.GetDateTime(6);
                    car.Mileage = reader.GetInt32(7);
                    car.Picture = reader.GetFieldValue<byte[]>(8);
                }
            }
            connection.Close();
            return car;
        }

        public List<CarCargoList> GetCarCargoLists(int carid)
        {
            List<CarCargoList> carCargoLists = new List<CarCargoList>();
            connection.Open();
            using (var command = new NpgsqlCommand(@"SELECT * FROM CarCargoList  WHERE carid=@carid", connection.Get()))
            {
                command.Parameters.AddWithValue("carid", carid);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CarCargoList carCargoList = new CarCargoList();
                    carCargoList.Car.Id = reader.GetInt32(0);
                    carCargoList.CargoType.Id = reader.GetInt32(1);
                    carCargoLists.Add(carCargoList);
                }
            }
            connection.Close();
            return carCargoLists;
        }

        public void InsertCarCargoList(List<CarCargoList> carCargoLists)
        {
            connection.Open();
            foreach (CarCargoList carCargoList in carCargoLists)
            {
                using (NpgsqlCommand command = new NpgsqlCommand(@"INSERT INTO CarCargoList(carid, cargoid) 
                    VALUES (@carid, @cargoid)", connection.Get()))
                {
                    command.Parameters.AddWithValue("carid", carCargoList.Car.Id);
                    command.Parameters.AddWithValue("cargoid", carCargoList.CargoType.Id);
                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
        }

        public void DeleteCarCargoList(int carid)
        {
            try
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("DELETE FROM CarCargoList WHERE carid=@carid",
                    connection.Get()))
                {
                    command.Parameters.AddWithValue("carid", carid);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка удаления");
            }
        }

        public int GetLastCarId()
        {
            int id;
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"SELECT carid FROM Car ORDER BY carid DESC LIMIT 1",
                connection.Get()))
            {
                id = (int)command.ExecuteScalar();
            }
            connection.Close();
            return id;
        }
    }
}
