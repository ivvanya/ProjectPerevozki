using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace ProjectPerevozki.Tools
{
    public class CarTools
    {
        private CarSql carSql;
        public CarTools()
        {
            carSql = CarSql.GetInstanse();
        }

        public void AddCarCargoList(List<CarCargoList> carCargoLists)
        {
            int carid = carSql.GetLastCarId();
            foreach (CarCargoList carCargoList in carCargoLists)
            {
                carCargoList.Car.Id = carid;
            }
            carSql.InsertCarCargoList(carCargoLists);
        }

        public void EditCarCargoList(List<CarCargoList> carCargoLists)
        {
            carSql.DeleteCarCargoList(carCargoLists[0].Car.Id);
            carSql.InsertCarCargoList(carCargoLists);
        }

        public byte[] ConvertPicToByte(Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        public string checkData(Car car)
        {
            string status;
            if (car.Number == "" || car.Brand == "" || car.Model == "")
                status = "Поля заполнены некорректно";
            else if (!CheckNumberExists(car.Number, car.Id))
                status = "Автомобиль с таким номером уже существует";
            else if (!CheckModel(car.Model))
                status = "Поле с названием модели заполнено некорректно";
            else if (!CheckBrand(car.Brand))
                status = "Поле с названием марки заполнено некорректно";
            else if (!CheckGosNumber(car.Number))
                status = "Поле с номером автомобиля заполнено некорректно";
            else
                status = "good";

            return status;
        }

        private bool CheckBrand(string name)
        {
            string pattern = @"^(([а-яa-z])+ ?)*$";
            if (Regex.IsMatch(name, pattern, RegexOptions.IgnoreCase))
                return true;
            return false;
        }
        private bool CheckModel(string name)
        {
            string pattern = @"^(([а-яa-z\d])+ ?)*$";
            if (Regex.IsMatch(name, pattern, RegexOptions.IgnoreCase))
                return true;
            return false;
        }
        private bool CheckGosNumber(string number)
        {
            string[] pattern ={
                @"^[а-я]{1}\d{3}[а-я]{2}(\d{2}|\d{3})$",
                @"^[а-я]{2}(\d{6}|\d{7})$",
                @"^[а-я]{2}(\d{5}|\d{6})$",
                @"^\d{4}[а-я]{2}(\d{2}|\d{3})$"};
            foreach (string s in pattern)
                if (Regex.IsMatch(number, s, RegexOptions.IgnoreCase))
                    return true;
            return false;
        }

        private bool CheckNumberExists(string number, int id)
        {
            foreach (Car car in carSql.SelectAll(id))
            {
                if (car.Number == number)
                    return false;
            }
            return true;
        }
    }
}
