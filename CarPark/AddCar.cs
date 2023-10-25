using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using ProjectPerevozki.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CarPark
{
    public partial class AddCar : Form
    {
        bool insertOrUpdate;
        int carId = 0;
        private List<CargoType> cargoTypes = new List<CargoType>();
        public AddCar()
        {
            insertOrUpdate = true;
            TopMost = true;
            InitializeComponent();
            pictureBox1.SendToBack();
        }

        public AddCar(Car car, AccessRights globalAccessRights = null)
        {
            carId = car.Id;
            insertOrUpdate = false;
            TopMost = true;
            InitializeComponent();

            textBox1.Text = car.Number;
            textBox2.Text = car.Brand;
            textBox3.Text = car.Model;
            numericUpDown1.Value = car.CapLoad;
            numericUpDown2.Value = car.Mileage;
            dateTimePicker1.Value = car.DateOfIssue;
            dateTimePicker2.Value = car.DateOfTo;
            using (MemoryStream ms = new MemoryStream(car.Picture))
            {
                ImageConverter converter = new ImageConverter();
                pictureBox1.Image = converter.ConvertFrom(car.Picture) as Image;
            }

            if (!globalAccessRights.Edit)
            {
                button1.Hide();
                button2.Hide();
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                numericUpDown1.Enabled = false;
                numericUpDown2.Enabled = false;
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Image Files(*.JPG; *.JPEG)|*.JPG; *.JPEG",
                ValidateNames = true,
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox1.Image = new Bitmap(openFileDialog.FileName);
                    label9.Hide();
                }
                catch
                {
                    MessageBox.Show("Невозможно открыть выбранный файл");
                    return;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CarTools carTools = new CarTools();
            Car car = new Car
            {
                Id = carId,
                Number = textBox1.Text,
                Brand = textBox2.Text,
                Model = textBox3.Text,
                CapLoad = Convert.ToInt32(numericUpDown1.Value),
                DateOfIssue = dateTimePicker1.Value,
                DateOfTo = dateTimePicker2.Value,
                Mileage = Convert.ToInt32(numericUpDown2.Value)
            };

            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Загрузите фото автомобиля");
                return;
            }
            else
                car.Picture = carTools.ConvertPicToByte(pictureBox1.Image);

            string status = carTools.checkData(car);
            if (status != "good")
            {
                MessageBox.Show(status);
                return;
            }
            else
            {
                CarSql carSql = new CarSql();
                if (insertOrUpdate)
                {
                    carSql.WriteToDb(car);
                    AddCargo newForm = new AddCargo(car, insertOrUpdate);
                    newForm.Show();
                }
                else
                {
                    carSql.UpdateInDb(car);
                    MessageBox.Show("Данные обновлены");
                }

                this.Close();
                DialogResult = DialogResult.OK;
            }
        }
    }
}
