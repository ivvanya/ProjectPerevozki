using System.Collections.Generic;

namespace ProjectPerevozki.Models
{
    public class MenuStructure
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string DllName { get; set; }
        public string MethodName { get; set; }
        public List<MenuStructure> SubMenu { get; set; }


        public MenuStructure()
        {
            SubMenu = new List<MenuStructure>();
        }
    }
}
