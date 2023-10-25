using Npgsql;
using ProjectPerevozki.Models;
using System.Collections.Generic;

namespace ProjectPerevozki.DataBase
{
    public class AccessRightsSql
    {
        private SQLConnection connection;
        public AccessRightsSql()
        {
            connection = SQLConnection.GetInstanse();
        }

        private static AccessRightsSql _instanse;

        public static AccessRightsSql GetInstanse()
        {
            if (_instanse == null)
            {
                _instanse = new AccessRightsSql();
            }
            return _instanse;
        }

        public AccessRights Check(int menuId, int employeeId)
        {
            AccessRights accessRights = new AccessRights();
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"SELECT * from AccessRights WHERE employeeid=@employeeid and menuid=@menuid", connection.Get()))
            {
                command.Parameters.AddWithValue("employeeid", employeeId);
                command.Parameters.AddWithValue("menuid", menuId);

                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    accessRights.MenuId = menuId;
                    accessRights.EmployeeId = employeeId;
                    accessRights.Read = reader.GetBoolean(2);
                    accessRights.Write = reader.GetBoolean(3);
                    accessRights.Edit = reader.GetBoolean(4);
                    accessRights.Delete = reader.GetBoolean(5);
                }
            }
            connection.Close();
            return accessRights;
        }
        public void WriteToDb(List<AccessRights> accessRightsList)
        {
            connection.Open();
            foreach (AccessRights accessRights in accessRightsList)
            {
                using (NpgsqlCommand command = new NpgsqlCommand(@"INSERT INTO AccessRights(employeeid, menuid, 
                    reada, writea, edita, deletea) VALUES (@EmpID, @MenuID, @R, @W, @E, @D)", connection.Get()))
                {
                    command.Parameters.AddWithValue("EmpID", accessRights.EmployeeId);
                    command.Parameters.AddWithValue("MenuID", accessRights.MenuId);
                    command.Parameters.AddWithValue("R", accessRights.Read);
                    command.Parameters.AddWithValue("W", accessRights.Write);
                    command.Parameters.AddWithValue("E", accessRights.Edit);
                    command.Parameters.AddWithValue("D", accessRights.Delete);
                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
        }

        public List<AccessRights> GetAccessRightsList(int empId)
        {
            List<AccessRights> accessRightsList = new List<AccessRights>();
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(@"SELECT a.menuid, m.name, reada, writea, edita, deletea 
                from AccessRights a JOIN menustructure m ON m.menuid=a.menuid WHERE employeeid=@id ORDER BY menuid;", connection.Get()))
            {
                command.Parameters.AddWithValue("id", empId);
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    AccessRights accessRights = new AccessRights();
                    accessRights.MenuId = reader.GetInt32(0);
                    accessRights.Menu.Id = reader.GetInt32(0);
                    accessRights.Menu.Name = reader.GetString(1);
                    accessRights.Read = reader.GetBoolean(2);
                    accessRights.Write = reader.GetBoolean(3);
                    accessRights.Edit = reader.GetBoolean(4);
                    accessRights.Delete = reader.GetBoolean(5);
                    accessRightsList.Add(accessRights);
                }
            }
            connection.Close();
            return accessRightsList;
        }

        public void UpdateAccessRightsList(List<AccessRights> accessRightsList)
        {
            connection.Open();
            foreach (AccessRights accessRights in accessRightsList)
            {
                using (NpgsqlCommand command = new NpgsqlCommand(@"UPDATE AccessRights SET reada = @R, writea=@W,
                    edita=@E, deletea=@D WHERE employeeid = @employeeid AND menuid=@menuid", connection.Get()))
                {
                    command.Parameters.AddWithValue("employeeid", accessRights.EmployeeId);
                    command.Parameters.AddWithValue("menuid", accessRights.MenuId);
                    command.Parameters.AddWithValue("R", accessRights.Read);
                    command.Parameters.AddWithValue("W", accessRights.Write);
                    command.Parameters.AddWithValue("E", accessRights.Edit);
                    command.Parameters.AddWithValue("D", accessRights.Delete);
                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
        }
    }
}
