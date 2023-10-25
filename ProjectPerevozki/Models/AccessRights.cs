namespace ProjectPerevozki.Models
{
    public class AccessRights
    {
        //public Employee employee { get; set; }
        public int EmployeeId { get; set; }
        public int MenuId { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }
        public bool Delete { get; set; }
        public bool Edit { get; set; }
        public MenuStructure Menu { get; set; }

        public AccessRights()
        {
            Menu = new MenuStructure();
            Read = false;
            Write = false;
            Delete = false;
            Edit = false;
        }
    }
}
