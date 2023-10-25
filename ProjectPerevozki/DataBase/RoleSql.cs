using Npgsql;
using ProjectPerevozki.Models;
using System.Collections.Generic;

namespace ProjectPerevozki.DataBase
{
    public class RoleSql
    {
        private SQLConnection connection;
        public RoleSql()
        {
            connection = SQLConnection.GetInstanse();
        }

        private static RoleSql _instanse;

        public static RoleSql GetInstanse()
        {
            if (_instanse == null)
            {
                _instanse = new RoleSql();
            }
            return _instanse;
        }

        public List<Role> SelectAllRoles()
        {
            List<Role> roles = new List<Role>();
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM Role", connection.Get()))
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Role role = new Role();
                    role.Id = reader.GetInt32(0);
                    role.Name = reader.GetString(1);
                    roles.Add(role);
                }
            }
            connection.Close();
            return roles;
        }

        public Role GetRole(string name)
        {
            Role role = new Role();
            connection.Open();
            using (var command = new NpgsqlCommand("SELECT * FROM Role WHERE Name=@Name", connection.Get()))
            {
                command.Parameters.AddWithValue("Name", name);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    role.Id = reader.GetInt32(0);
                    role.Name = reader.GetString(1);
                }
            }
            connection.Close();
            return role;
        }
    }
}
