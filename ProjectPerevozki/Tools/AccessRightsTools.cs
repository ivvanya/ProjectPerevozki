using ProjectPerevozki.DataBase;

namespace ProjectPerevozki.Tools
{
    public class AccessRightsTools
    {
        private MenuStructureSql menuStructureSql;
        public AccessRightsTools()
        {
            menuStructureSql = MenuStructureSql.GetInstanse();
        }

    }
}
