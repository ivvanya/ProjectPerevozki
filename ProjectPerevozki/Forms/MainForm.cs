using Npgsql;
using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace ProjectPerevozki
{
    public partial class MainForm : Form
    {
        private Dictionary<string, string> fileNames = new Dictionary<string, string>();
        private List<MenuStructure> menus = new List<MenuStructure>();
        private MenuStructure menu = new MenuStructure();
        private MenuStructure menuHead = new MenuStructure();
        private List<MenuStructure> previos = new List<MenuStructure>();

        private AccessRights globalAccessRights;
        private int level = 0;
        private ToolStripMenuItem tool = new ToolStripMenuItem();
        private ToolStripMenuItem toolHead;
        private List<ToolStripMenuItem> toolsHead = new List<ToolStripMenuItem>();
        private List<ToolStripMenuItem> result = new List<ToolStripMenuItem>();
        private Dictionary<ToolStripMenuItem, int> previosToolsWithLevel = new Dictionary<ToolStripMenuItem, int>();
        private MenuStructure m = new MenuStructure();
        private MenuStructure resulst = new MenuStructure();
        private Employee employee;
        private bool flag = false;

        public MainForm(Employee employee)
        {
            this.employee = employee;
            InitializeComponent();
            label1.Text = "Сотрудник в сети:\n" + employee.Name + "\n\nДолжность:\n" + employee.role.Name;
            SQLConnection sqlconnection = SQLConnection.GetInstanse();
            MenuStructure m = new MenuStructure();
            sqlconnection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM MenuStructure ORDER BY parentid, MenuIndex", sqlconnection.Get()))
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    MenuStructure menu = new MenuStructure();
                    menu.Id = reader.GetInt32(0);
                    menu.ParentId = reader.GetInt32(1);
                    menu.Name = reader.GetString(2);
                    if (reader.GetValue(3).ToString() == string.Empty)
                    {
                        menu.DllName = string.Empty;
                        menu.MethodName = string.Empty;
                    }
                    else
                    {
                        menu.DllName = reader.GetString(3);
                        menu.MethodName = reader.GetString(4);
                    }
                    m = AddToList(menu);
                }
            }
            sqlconnection.Close();
            ShowMenu(menus);
            FormClosing += MainForm_FormClosing;
        }

        private void ClosingM(Object sender, FormClosingEventArgs e)
        {
            if (!flag)
            {
                Application.Exit();
                flag = false;
            }
        }

        private void ShowMenu(List<MenuStructure> menus)
        {
            for (int i = 0; i < menus.Count; i++)
            {
                ToolStripMenuItem tool = new ToolStripMenuItem(menus[i].Name);
                if (menus[i].MethodName != string.Empty)
                {
                    fileNames.Add(menus[i].MethodName, menus[i].Name);
                    tool.Click += Click;
                }
                if (menus[i].SubMenu.Count > 0)
                {
                    if (toolHead == null)
                    {
                        toolHead = tool;
                        previosToolsWithLevel.Add(tool, level);
                        level++;
                        ShowMenu(menus[i].SubMenu);
                        level--;
                        result.Add(toolHead);
                        toolHead = null;
                        continue;
                    }
                    else
                    {
                        previosToolsWithLevel.Add(tool, level);
                        this.tool = previosToolsWithLevel.LastOrDefault(x => x.Value == level - 1).Key;
                        this.tool.DropDownItems.Add(tool);

                        if (toolHead != this.tool)
                        {
                            if (menus[i].SubMenu.Count == 3)
                            {
                                toolsHead.Add(this.tool);
                                toolHead.DropDownItems.AddRange(toolsHead.ToArray());
                            }
                        }
                        level++;
                        ShowMenu(menus[i].SubMenu);
                        level--;
                        continue;
                    }
                }

                if (toolHead != null)
                {
                    if (level == 1)
                    {
                        toolHead.DropDownItems.Add(tool);
                    }
                    else
                    {
                        var t = previosToolsWithLevel.LastOrDefault(x => x.Value == level - 1);
                        this.tool = t.Key;
                        this.tool.DropDownItems.Add(tool);
                    }
                }
                else
                {
                    if (result.Count > 0)
                    {
                        foreach (var item in result)
                        {
                            menuStrip1.Items.Add(item);
                        }
                        toolsHead = new List<ToolStripMenuItem>();
                        result = new List<ToolStripMenuItem>();
                    }
                    menuStrip1.Items.Add(tool);
                }
            }
            if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    menuStrip1.Items.Add(item);
                }
            }
        }

        private MenuStructure getFuncName(MenuStructure menu, string str)
        {
            if (menu.Name == str)
            {
                resulst = menu;
                return resulst;
            }
            else
            {
                for (int i = 0; i < menu.SubMenu.Count; i++)
                {
                    if (menu.Name == str)
                    {
                        resulst = menu;
                        break;
                    }
                    else
                    {
                        resulst = getFuncName(menu.SubMenu[i], str);
                    }
                }
                return resulst;
            }
        }

        private new void Click(object sender, EventArgs e)
        {
            MenuStructure menu = null;
            foreach (var item in fileNames)
            {
                if (item.Value == ((ToolStripMenuItem)sender).Text)
                {
                    menu = menus.FirstOrDefault(x => x.Name == item.Value);
                    if (menu == null)
                    {
                        for (int i = 0; i < menus.Count; i++)
                        {
                            menu = getM(((ToolStripMenuItem)sender).Text, menus[i]);
                        }
                        if (menu == null)
                            menu = menus.FirstOrDefault(x => x.Name == item.Key);
                        menu = getFuncName(menu, ((ToolStripMenuItem)sender).Text);
                    }
                    break;
                }
            }
            AccessRightsSql accessRightsSql = new AccessRightsSql();
            AccessRights accessRights = accessRightsSql.Check(menu.ParentId, employee.Id);
            globalAccessRights = accessRights;

            if (menu.ParentId == 1)
            {
                if (!accessRights.Write)
                {
                    MessageBox.Show("У вас недостаточно прав");
                    return;
                }
            }
            else if (menu.ParentId == 2)
            {
                if (!accessRights.Read)
                {
                    MessageBox.Show("У вас недостаточно прав");
                    return;
                }
                else if (menu.Id == 11)
                {
                    if (!accessRights.Write)
                    {
                        MessageBox.Show("У вас недостаточно прав");
                        return;
                    }
                }
            }
            else if (menu.ParentId == 3)
            {
                if (!accessRights.Read)
                {
                    MessageBox.Show("У вас недостаточно прав");
                    return;
                }
                else if (menu.Id == 13)
                {
                    if (!accessRights.Write)
                    {
                        MessageBox.Show("У вас недостаточно прав");
                        return;
                    }
                }
            }
            else if (menu.ParentId == 4)
            {
                if (!accessRights.Read)
                {
                    MessageBox.Show("У вас недостаточно прав");
                    return;
                }
                else if (menu.Id == 15)
                {
                    if (!accessRights.Write)
                    {
                        MessageBox.Show("У вас недостаточно прав");
                        return;
                    }
                }
                else if (menu.Id == 17)
                {
                    if (!accessRights.Write)
                    {
                        MessageBox.Show("У вас недостаточно прав");
                        return;
                    }
                }
            }
            else if (menu.ParentId == 5)
            {
                if (!accessRights.Read)
                {
                    MessageBox.Show("У вас недостаточно прав");
                    return;
                }
            }
            else if (menu.ParentId == 6)
            {
                if (!accessRights.Read)
                {
                    MessageBox.Show("У вас недостаточно прав");
                    return;
                }
            }
            else if (menu.ParentId == 7)
            {
                if (!accessRights.Read)
                {
                    MessageBox.Show("У вас недостаточно прав");
                    return;
                }
            }
            else if (menu.ParentId == 8)
            {
                if (!accessRights.Read)
                {
                    MessageBox.Show("У вас недостаточно прав");
                    return;
                }
            }

            Assembly assembly = Assembly.LoadFrom("../../../" + menu.DllName + "/bin/Debug/" + menu.DllName + ".dll"); //debug
            //Assembly assembly = Assembly.LoadFrom(menu.DllName + ".dll"); //release
            var b = assembly.GetTypes();
            var type = assembly.GetType(menu.DllName + "." + menu.MethodName);
            if (type != null)
            {
                object obj = Activator.CreateInstance(type, globalAccessRights);
                MethodInfo init = type.GetMethods().FirstOrDefault(x => x.Name == menu.MethodName);
                if (obj is Form form)
                {
                    form.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Error");
            }
        }

        private MenuStructure getM(string text, MenuStructure m)
        {
            if (m.Name == text)
            {
                resulst = m;
                return resulst;
            }
            if (m.SubMenu.Count > 0)
            {
                for (int i = 0; i < m.SubMenu.Count; i++)
                {
                    getM(text, m.SubMenu[i]);
                }
            }

            return resulst;
        }

        private MenuStructure AddToList(MenuStructure menuStructure)
        {
            if (menuStructure.DllName == string.Empty)
            {
                if (menuStructure.ParentId > this.menu.ParentId)
                {
                    this.menu.SubMenu.Add(menu);
                }
                else if (menuStructure.ParentId > menuHead.ParentId)
                {
                    var item = previos.LastOrDefault(x => x.ParentId == menu.ParentId - 1);
                    item.SubMenu.Add(menu);
                }
                else
                {
                    menuHead = menuStructure;
                    menus.Add(menuHead);
                }
                previos.Add(menuStructure);
                this.menu = menuStructure;
            }
            else
            {
                if (menuStructure.ParentId > menuHead.ParentId)
                {
                    m = previos.LastOrDefault(x => x.Id == menuStructure.ParentId);
                    m.SubMenu.Add(menuStructure);
                }
                else
                {
                    menus.Add(menuStructure);
                }
            }
            return m;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
