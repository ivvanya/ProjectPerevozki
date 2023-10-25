using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Employee
{
    public partial class AddAccessRights : Form
    {
        private int employeeID;
        private bool InsertOrUpdate;
        public AddAccessRights(string log)
        {
            MenuStructureSql menuStructureSql = new MenuStructureSql();
            EmployeeSql employeeSql = new EmployeeSql();

            ProjectPerevozki.Models.Employee employee = new ProjectPerevozki.Models.Employee();
            employee = employeeSql.GetEmployeeByLog(log);
            employeeID = employee.Id;
            InsertOrUpdate = true;

            TopMost = true;
            InitializeComponent();

            List<MenuStructure> menuStructures = menuStructureSql.SelectAllMenuStructure();
            foreach (MenuStructure menuStructure in menuStructures)
                if (menuStructure.ParentId == 0)
                    dataGridView1.Rows.Add(menuStructure.Id, menuStructure.Name);
        }

        public AddAccessRights(int empId)
        {
            AccessRightsSql accessRightsSql = new AccessRightsSql();
            List<AccessRights> accessRightsList = new List<AccessRights>();

            InsertOrUpdate = false;
            employeeID = empId;
            InitializeComponent();

            accessRightsList = accessRightsSql.GetAccessRightsList(employeeID);
            foreach (AccessRights aR in accessRightsList)
                dataGridView1.Rows.Add(aR.Menu.Id, aR.Menu.Name, aR.Read, aR.Write, aR.Edit, aR.Delete);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AccessRightsSql accessRightsSql = new AccessRightsSql();
            List<AccessRights> accessRightsList = new List<AccessRights>();

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                AccessRights accessRights = new AccessRights();
                accessRights.EmployeeId = employeeID;
                accessRights.MenuId = Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value);
                accessRights.Read = Convert.ToBoolean(dataGridView1.Rows[i].Cells[2].Value);
                accessRights.Write = Convert.ToBoolean(dataGridView1.Rows[i].Cells[3].Value);
                accessRights.Edit = Convert.ToBoolean(dataGridView1.Rows[i].Cells[4].Value);
                accessRights.Delete = Convert.ToBoolean(dataGridView1.Rows[i].Cells[5].Value);
                accessRightsList.Add(accessRights);
            }
            if (InsertOrUpdate)
                accessRightsSql.WriteToDb(accessRightsList);
            else
                accessRightsSql.UpdateAccessRightsList(accessRightsList);

            MessageBox.Show("Изменение выполнено успешно");
            this.Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    dataGridView1.Rows[i].Cells[2].Value = true;
            else
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    dataGridView1.Rows[i].Cells[2].Value = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    dataGridView1.Rows[i].Cells[3].Value = true;
            else
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    dataGridView1.Rows[i].Cells[3].Value = false;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    dataGridView1.Rows[i].Cells[4].Value = true;
            else
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    dataGridView1.Rows[i].Cells[4].Value = false;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    dataGridView1.Rows[i].Cells[5].Value = true;
            else
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    dataGridView1.Rows[i].Cells[5].Value = false;
        }
    }
}
