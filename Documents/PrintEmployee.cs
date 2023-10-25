using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using System;
using System.Windows.Forms;

namespace Documents
{
    public partial class PrintEmployee : Form
    {
        private AccessRights globalAccessRights;
        public PrintEmployee(AccessRights globalAccessRights)
        {
            this.globalAccessRights = globalAccessRights;
            InitializeComponent();
            UpdateTable();
        }

        private void UpdateTable()
        {
            EmployeeSql employeeSql = new EmployeeSql();
            foreach (Employee employee in employeeSql.EmployeeWithRoles())
            {
                dataGridView1.Rows.Add(employee.Name, employee.PhoneNumber, employee.Email, employee.role.Name, employee.Login);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                Microsoft.Office.Interop.Excel.Application xcelApp = new Microsoft.Office.Interop.Excel.Application();
                xcelApp.Application.Workbooks.Add(Type.Missing);
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    xcelApp.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
                }
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        xcelApp.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }
                xcelApp.Columns.AutoFit();
                xcelApp.Visible = true;
            }
        }
    }
}
