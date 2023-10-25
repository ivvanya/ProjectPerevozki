using ProjectPerevozki.Models;
using ProjectPerevozki.Tools;
using System;
using System.Windows.Forms;

namespace Client
{
    public partial class AddIndividual : Form
    {
        private AccessRights globalAccessRights;
        public AddIndividual(AccessRights globalAccessRights)
        {
            this.globalAccessRights = globalAccessRights;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Individual individual = new Individual
            {
                Name = textBox1.Text,
                Number = textBox2.Text,
                PassSeries = textBox3.Text,
                PassNumber = textBox4.Text,
                DateOfIssue = dateTimePicker1.Value,
                IssuedBy = textBox5.Text
            };


            IndividualTools individualTools = new IndividualTools();
            string status = individualTools.checkData(individual);

            if (status != "good")
            {
                MessageBox.Show(status);
                return;
            }
            else
            {
                individualTools.AddIndividual(individual);
                MessageBox.Show("Клиент добавлен");
                this.Hide();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
