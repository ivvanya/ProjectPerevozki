namespace ProjectPerevozki.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int RoleID { get; set; }
        public Role role { get; set; }

        public Employee()
        {
            role = new Role();
            Id = 0;
        }
    }
}
