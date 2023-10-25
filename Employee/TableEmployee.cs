using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using ProjectPerevozki.Tools;
using System;
using System.Windows.Forms;

namespace Employee
{
    public partial class TableEmployee : Form
    {
        private AccessRights globalAccessRights;
        public TableEmployee(AccessRights globalAccessRights)
        {
            this.globalAccessRights = globalAccessRights;
            InitializeComponent();
            UpdateTable();
        }

        private void UpdateRoleColumn()
        {
            RoleSql roleSql = new RoleSql();
            foreach (Role role in roleSql.SelectAllRoles())
            {
                Column6.Items.Add(role.Name);
            }
        }

        private void UpdateTable()
        {
            UpdateRoleColumn();

            dataGridView1.Rows.Clear();
            EmployeeSql employeeSql = new EmployeeSql();
            foreach (ProjectPerevozki.Models.Employee emp in employeeSql.EmployeeWithRoles())
            {
                dataGridView1.Rows.Add(emp.Id, emp.Name, emp.Email, emp.PhoneNumber,
                    emp.role.Id, emp.role.Name, emp.Login);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (globalAccessRights.Edit)
            {
                AddAccessRights newForm = new AddAccessRights(Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value));
                newForm.Show();
            }
            else
            {
                MessageBox.Show("У вас недостаточно прав");
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (globalAccessRights.Delete)
            {
                DialogResult result = MessageBox.Show("Вы точно хотите удалить запись?",
                "Предупреждение", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    EmployeeSql employeeSql = new EmployeeSql();
                    employeeSql.DeleteFromDb(Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value));
                    UpdateTable();
                }
                else return;
            }
            else
            {
                MessageBox.Show("У вас недостаточно прав");
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (globalAccessRights.Edit)
            {
                ProjectPerevozki.Models.Employee employee = new ProjectPerevozki.Models.Employee();
                employee.Id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                employee.Name = Convert.ToString(dataGridView1.CurrentRow.Cells[1].Value);
                employee.Email = Convert.ToString(dataGridView1.CurrentRow.Cells[2].Value);
                employee.PhoneNumber = Convert.ToString(dataGridView1.CurrentRow.Cells[3].Value);
                employee.role.Name = Convert.ToString(dataGridView1.CurrentRow.Cells[5].Value);
                employee.Login = Convert.ToString(dataGridView1.CurrentRow.Cells[6].Value);

                EmployeeTools employeeTools = new EmployeeTools();
                string status = employeeTools.checkData(employee);

                if (status != "good")
                {
                    MessageBox.Show(status);
                    return;
                }
                else
                {
                    employeeTools.UpdateEmployee(employee);
                    MessageBox.Show("Данные пользователя обновлены");
                    UpdateTable();
                }
            }
            else
            {
                MessageBox.Show("У вас недостаточно прав");
                return;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
