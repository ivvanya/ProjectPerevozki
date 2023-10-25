using ProjectPerevozki.Models;
using ProjectPerevozki.Tools;
using System;
using System.Windows.Forms;

namespace Client
{
    public partial class AddEntity : Form
    {
        private AccessRights globalAccessRights;

        public AddEntity(AccessRights globalAccessRights)
        {
            this.globalAccessRights = globalAccessRights;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Entity entity = new Entity
            {
                Name = textBox1.Text,
                HeadName = textBox2.Text,
                Number = textBox3.Text,
                Address = textBox4.Text,
                Bank = textBox5.Text,
                BankAccount = textBox6.Text,
                INN = textBox7.Text
            };


            EntityTools entityTools = new EntityTools();
            string status = entityTools.checkData(entity);

            if (status != "good")
            {
                MessageBox.Show(status);
                return;
            }
            else
            {
                entityTools.AddEntity(entity);
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
