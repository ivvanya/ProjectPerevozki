namespace ProjectPerevozki.Models
{
    public class DriverClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DriverClass()
        {
            Id = 0;
        }
    }
}