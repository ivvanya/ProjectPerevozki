using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using ProjectPerevozki.Tools;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Contract
{
    public partial class TableTrip : Form
    {
        ProjectPerevozki.Models.Contract contract;
        List<List<TripDriverList>> tripDriverLists = new List<List<TripDriverList>>();
        List<List<TripCargoList>> tripCargoLists = new List<List<TripCargoList>>();
        List<Trip> trips = new List<Trip>();
        private AccessRights globalAccessRights;

        public TableTrip(ProjectPerevozki.Models.Contract contract, AccessRights globalAccessRights)
        {
            this.globalAccessRights = globalAccessRights;
            this.contract = contract;
            TopMost = true;
            InitializeComponent();
            if (contract.Id != 0)
            {
                button1.Hide();
                button2.Hide();
                button3.Text = "Выход";
                UpdateTable();
                button4.Location = button1.Location;
                button5.Location = button2.Location;
            }
            else
            {
                button4.Hide();
                button5.Hide();
            }
        }

        private void UpdateTable()
        {
            TripSql tripSql = new TripSql();
            foreach (Trip trip in tripSql.SelectAllTrips(contract.Id))
            {
                dataGridView1.Rows.Add(trip.Id, trip.Contract.Id, trip.Car.Number + ", " + trip.Car.Brand + " " + trip.Car.Model, trip.InputTime);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddTrip addTrip = new AddTrip();
            if (addTrip.ShowDialog() == DialogResult.OK)
            {
                List<TripCargoList> tripCargoList;
                Trip trip;
                (trip, tripCargoList) = addTrip.GetInformation();
                trips.Add(trip);
                tripCargoLists.Add(tripCargoList);
                addTrip.Close();

                dataGridView1.Rows.Add(trip.Id, contract.Id, trip.Car.Number + ", " + trip.Car.Brand + " " + trip.Car.Model, trip.InputTime);

                AddDriver addDriver = new AddDriver();
                if (addDriver.ShowDialog() == DialogResult.OK)
                {
                    List<TripDriverList> tripDriverList;
                    tripDriverList = addDriver.GetInformation();
                    tripDriverLists.Add(tripDriverList);
                    addDriver.Close();
                }
                else
                {
                    trips.RemoveAt(trips.Count - 1);
                    tripCargoLists.RemoveAt(tripCargoLists.Count - 1);
                    dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (contract.Id == 0)
            {
                TripTools tripTools = new TripTools();
                tripTools.AddFullContract(contract, trips, tripDriverLists, tripCargoLists);
                this.Close();
                MessageBox.Show("Договор добавлен");
            }
            else
            {
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            trips.RemoveAt(dataGridView1.CurrentRow.Index);
            tripDriverLists.RemoveAt(dataGridView1.CurrentRow.Index);
            tripCargoLists.RemoveAt(dataGridView1.CurrentRow.Index);
            dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddTrip addTrip = new AddTrip(Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value));
            addTrip.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AddDriver addDriver = new AddDriver(Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value));
            addDriver.Show();
        }
    }
}
