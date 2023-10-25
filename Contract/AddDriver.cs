using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Contract
{
    public partial class AddDriver : Form
    {
        private List<TripDriverList> tripDriverLists;
        public AddDriver(int tripid = 0)
        {
            TopMost = true;
            InitializeComponent();

            if (tripid != 0)
            {
                TripSql tripSql = new TripSql();
                foreach (TripDriverList tripDriverList in tripSql.SelectAllDriversByTripId(tripid))
                    dataGridView1.Rows.Add(0, true, tripDriverList.Driver.Name, tripDriverList.Driver.Experience,
                        tripDriverList.Driver.DriverClass.Name);
                button1.Hide();
            }
            else
            {
                UpdateTable();
            }
        }
        private void UpdateTable()
        {
            dataGridView1.Rows.Clear();
            DriverSql driverSql = new DriverSql();
            foreach (Driver driver in driverSql.SelectAll())
                dataGridView1.Rows.Add(driver.Id, false, driver.Name, driver.Experience, driver.DriverClass.Name);
        }

        public List<TripDriverList> GetInformation()
        {
            return tripDriverLists;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int countTrue = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[1].Value) == true)
                    countTrue++;

            if (countTrue < 1)
            {
                MessageBox.Show("Выберите хотя бы одного водителя");
            }
            else
            {
                tripDriverLists = new List<TripDriverList>();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[1].Value) == true)
                    {
                        TripDriverList tripDriverList = new TripDriverList();
                        tripDriverList.Driver.Id = Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value);
                        tripDriverLists.Add(tripDriverList);
                        DialogResult = DialogResult.OK;
                    }
            }
        }
    }
}
