using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using ProjectPerevozki.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Contract
{
    public partial class AddTrip : Form
    {
        private Trip trip;
        private List<TripCargoList> TripCargoLists = new List<TripCargoList>();

        public AddTrip(int tripid = 0)
        {
            TopMost = true;
            InitializeComponent();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd.MMMM.yyyy HH:mm";

            CargoTypeSql cargoTypeSql = new CargoTypeSql();
            foreach (CargoType cargoType in cargoTypeSql.SelectAll())
            {
                comboBox1.Items.Add(cargoType.Name);
            }

            UpdateCarGrid();

            if (tripid != 0)
            {
                TripSql tripSql = new TripSql();
                foreach (TripCargoList tripCargoList in tripSql.SelectAllCargoByTripId(tripid))
                    dataGridView2.Rows.Add(0, tripCargoList.CargoType.Name, tripCargoList.Name, tripCargoList.Amount,
                        tripCargoList.Weight, tripCargoList.Insurance);

                int carid = tripSql.GetCarIdByTripId(tripid);
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    if (Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value) == carid)
                    {
                        dataGridView1.Rows[i].Cells[1].Value = true;
                        break;
                    }
                button1.Hide();
                button2.Hide();
                button3.Hide();
                groupBox4.Enabled = false;
            }
        }

        private void UpdateCarGrid()
        {
            CarSql carSql = new CarSql();
            foreach (Car car in carSql.SelectAll())
                dataGridView1.Rows.Add(car.Id, false, car.Number, car.Brand + " " + car.Model, car.CapLoad);
        }

        private void UpdateColorCarGrid()
        {
            CarSql carSql = new CarSql();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewCellStyle rowDefault = new DataGridViewCellStyle();
                rowDefault.BackColor = Color.White;
                rowDefault.SelectionBackColor = Color.DodgerBlue;
                dataGridView1.Rows[i].ReadOnly = false;
                dataGridView1.Rows[i].DefaultCellStyle = rowDefault;

                List<CarCargoList> carCargoLists = carSql.GetCarCargoLists(Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value));
                List<(int, int)> carList = new List<(int, int)>();

                foreach (CarCargoList carCargoList in carCargoLists)
                {
                    carList.Add((carCargoList.Car.Id, carCargoList.CargoType.Id));
                }

                int totalWeight = 0;
                foreach (TripCargoList tripCargoList in TripCargoLists)
                {
                    (int, int) tripList = (Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value), tripCargoList.CargoType.Id);

                    if (!carList.Contains(tripList))
                    {
                        DataGridViewCellStyle rowRed = new DataGridViewCellStyle();
                        rowRed.BackColor = Color.OrangeRed;
                        rowRed.SelectionBackColor = Color.Red;
                        dataGridView1.Rows[i].ReadOnly = true;
                        dataGridView1.Rows[i].DefaultCellStyle = rowRed;
                        dataGridView1.Rows[i].Cells[1].Value = false;
                    }

                    totalWeight += tripCargoList.Weight;
                }

                if (totalWeight > Convert.ToInt32(dataGridView1.Rows[i].Cells[4].Value))
                {
                    DataGridViewCellStyle rowRed = new DataGridViewCellStyle();
                    rowRed.BackColor = Color.OrangeRed;
                    rowRed.SelectionBackColor = Color.Red;
                    dataGridView1.Rows[i].ReadOnly = true;
                    dataGridView1.Rows[i].DefaultCellStyle = rowRed;
                    dataGridView1.Rows[i].Cells[1].Value = false;
                }
            }
        }

        public (Trip, List<TripCargoList>) GetInformation()
        {
            return (trip, TripCargoLists);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TripSql tripSql = new TripSql();
            TripCargoList tripCargoList = new TripCargoList();
            tripCargoList.CargoType.Name = comboBox1.Text;
            tripCargoList.CargoType.Id = tripSql.GetCargoTypeId(comboBox1.Text);
            tripCargoList.Name = textBox1.Text;
            tripCargoList.Amount = Convert.ToInt32(numericUpDown1.Value);
            tripCargoList.Weight = Convert.ToInt32(numericUpDown2.Value);
            tripCargoList.Insurance = Convert.ToInt32(numericUpDown3.Value);

            TripTools tripTools = new TripTools();
            string status = tripTools.checkData(tripCargoList);
            if (status == "good")
            {
                dataGridView2.Rows.Add(tripCargoList.Id, tripCargoList.CargoType.Name, tripCargoList.Name, tripCargoList.Amount,
                    tripCargoList.Weight, tripCargoList.Insurance);
                TripCargoLists.Add(tripCargoList);
                UpdateColorCarGrid();
            }
            else
            {
                MessageBox.Show(status);
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TripCargoLists.RemoveAt(dataGridView2.CurrentRow.Index);
            dataGridView2.Rows.Remove(dataGridView2.CurrentRow);
            UpdateColorCarGrid();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int countTrue = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[1].Value) == true)
                {
                    countTrue++;
                }
            }
            if (countTrue != 1)
            {
                MessageBox.Show("Выберите 1 автомобиль");
                return;
            }
            else if (dataGridView2.Rows.Count < 1)
            {
                MessageBox.Show("Добавьте хотя бы 1 груз");
                return;
            }
            else
            {
                trip = new Trip();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[1].Value) == true)
                    {
                        trip.Car.Id = Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value);
                        trip.Car.Number = Convert.ToString(dataGridView1.Rows[i].Cells[2].Value);
                        trip.Car.Brand = Convert.ToString(dataGridView1.Rows[i].Cells[3].Value).Split(' ')[0];
                        trip.Car.Model = Convert.ToString(dataGridView1.Rows[i].Cells[3].Value).Split(' ')[1];
                        trip.InputTime = dateTimePicker1.Value;
                        break;
                    }
                }

                DialogResult = DialogResult.OK;
            }
        }
    }
}
