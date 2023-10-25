using Npgsql;
using System.Configuration;

namespace ProjectPerevozki.DataBase
{
    internal class SQLConnection
    {
        private NpgsqlConnection connection;
        private static SQLConnection _instanse;

        public static SQLConnection GetInstanse()
        {
            if (_instanse == null)
            {
                _instanse = new SQLConnection();
            }
            return _instanse;
        }

        private SQLConnection()
        {
            connection = new NpgsqlConnection(ConfigurationManager.
                ConnectionStrings["ProjectArenda.Properties.Settings.DatabaseConnectionString"].ConnectionString);
        }

        public NpgsqlConnection Get()
        {
            return connection;
        }

        public void Open()
        {
            connection.Open();
        }

        public void Close()
        {
            connection.Close();
        }
    }
}
