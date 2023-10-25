using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Reference
{
    public partial class Content : Form
    {
        private AccessRights globalAccessRights;
        public Content(AccessRights globalAccessRights)
        {
            this.globalAccessRights = globalAccessRights;
            InitializeComponent();
            MenuStructureSql menuStructureSql = new MenuStructureSql();
            List<MenuStructure> menuStructures = menuStructureSql.SelectAllMenuStructure();
            listBox1.Items.Clear();
            foreach (MenuStructure menuStructure in menuStructures)
            {
                if (menuStructure.ParentId == 0)
                {
                    listBox1.Items.Add(menuStructure.Id + ". " + menuStructure.Name);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            textBox1.Clear();
            if (index == 0)
            {
                textBox1.Text = "Содержит в себе инструменты, позволяющие управлять текущей учетной записью - " +
                    "смена пароля, логина или персональных данных пользователя.";
            }
            else if (index == 1)
            {
                textBox1.Text = "Содержит в себе инструменты, позволяющие управлять текущими заказами или создавать новые.";
            }
            else if (index == 2)
            {
                textBox1.Text = "Содержит в себе инструменты, позволяющие управлять учетными записями пользователей, " +
                    "правами доступа или создавать новые.";
            }
            else if (index == 3)
            {
                textBox1.Text = "Содержит в себе инструменты, позволяющие управлять данными о клиентах.";
            }
            else if (index == 4)
            {
                textBox1.Text = "Содержит в себе инструменты, позволяющие управлять данными об автомобилях, предназначенных для грузоперевозок, " +
                    "а также о водителях";
            }
            else if (index == 5)
            {
                textBox1.Text = "Содержит в себе инструменты, позволяющие создавать выходные документы в формате MS Office.";
            }
            else if (index == 6)
            {
                textBox1.Text = "Содержит в себе справку.";
            }
            else if (index == 7)
            {
                textBox1.Text = "Содержит в себе инструменты, позволяющие управлять справочниками базы данных.";
            }
        }
    }
}
