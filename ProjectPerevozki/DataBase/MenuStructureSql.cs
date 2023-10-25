using Npgsql;
using ProjectPerevozki.Models;
using System.Collections.Generic;

namespace ProjectPerevozki.DataBase
{
    public class MenuStructureSql
    {
        private SQLConnection connection;
        public MenuStructureSql()
        {
            connection = SQLConnection.GetInstanse();
        }

        private static MenuStructureSql _instanse;

        public static MenuStructureSql GetInstanse()
        {
            if (_instanse == null)
            {
                _instanse = new MenuStructureSql();
            }
            return _instanse;
        }

        public List<MenuStructure> SelectAllMenuStructure()
        {
            List<MenuStructure> menuStructures = new List<MenuStructure>();
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM MenuStructure ORDER BY parentid, MenuIndex", connection.Get()))
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    MenuStructure menuStructure = new MenuStructure();
                    menuStructure.Id = reader.GetInt32(0);
                    menuStructure.ParentId = reader.GetInt32(1);
                    menuStructure.Name = reader.GetString(2);
                    if (reader.GetValue(3).ToString() == string.Empty)
                    {
                        menuStructure.DllName = string.Empty;
                        menuStructure.MethodName = string.Empty;
                    }
                    else
                    {
                        menuStructure.DllName = reader.GetString(3);
                        menuStructure.MethodName = reader.GetString(4);
                    }
                    menuStructures.Add(menuStructure);
                }
            }
            connection.Close();
            return menuStructures;
        }

        public List<MenuStructure> SelectByParentId(int parentId)
        {
            List<MenuStructure> menuStructures = new List<MenuStructure>();
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM MenuStructure ORDER BY parentid, MenuIndex WHERE ParentID = @ID", connection.Get()))
            {
                command.Parameters.AddWithValue("ID", parentId);
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    MenuStructure menuStructure = new MenuStructure();
                    menuStructure.Id = reader.GetInt32(0);
                    menuStructure.ParentId = reader.GetInt32(1);
                    menuStructure.Name = reader.GetString(2);
                    if (reader.GetValue(3).ToString() == string.Empty)
                    {
                        menuStructure.DllName = string.Empty;
                        menuStructure.MethodName = string.Empty;
                    }
                    else
                    {
                        menuStructure.DllName = reader.GetString(3);
                        menuStructure.MethodName = reader.GetString(4);
                    }
                    menuStructures.Add(menuStructure);
                }
            }
            connection.Close();
            return menuStructures;
        }
    }
}
