using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using ProjectPerevozki.Tools;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CarPark
{
    public partial class AddCargo : Form
    {
        bool insertOrUpdate;
        Car car;
        public AddCargo(Car car, bool insertOrUpdate)
        {
            this.insertOrUpdate = insertOrUpdate;
            this.car = car;
            TopMost = true;
            InitializeComponent();
            UpdateTable();
        }

        public AddCargo(Car car, bool insertOrUpdate, AccessRights globalAccessRights)
        {
            this.insertOrUpdate = insertOrUpdate;
            this.car = car;
            TopMost = true;
            InitializeComponent();
            UpdateTable();

            if (!globalAccessRights.Edit)
            {
                button1.Hide();
                dataGridView1.Enabled = false;
            }
        }

        private void UpdateTable()
        {
            dataGridView1.Rows.Clear();
            CargoTypeSql cargoTypeSql = new CargoTypeSql();

            foreach (CargoType cargoType in cargoTypeSql.SelectAll())
                dataGridView1.Rows.Add(cargoType.Id, false, cargoType.Name, cargoType.Description);

            if (!insertOrUpdate)
            {
                CarSql carSql = new CarSql();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    foreach (CarCargoList carCargoList in carSql.GetCarCargoLists(car.Id))
                    {
                        if (Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value) == carCargoList.CargoType.Id)
                            dataGridView1.Rows[i].Cells[1].Value = true;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<CarCargoList> carCargoLists = new List<CarCargoList>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[1].Value) == true)
                {
                    CarCargoList carCargoList = new CarCargoList();
                    carCargoList.Car.Id = car.Id;
                    carCargoList.CargoType.Id = Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value);
                    carCargoLists.Add(carCargoList);
                }
            }

            CarTools carTools = new CarTools();
            if (insertOrUpdate)
            {
                carTools.AddCarCargoList(carCargoLists);
                MessageBox.Show("Данные добавлены");
                this.Close();
            }
            else
            {
                carTools.EditCarCargoList(carCargoLists);
                MessageBox.Show("Данные обновлены");
                this.Close();
            }
        }
    }
}
