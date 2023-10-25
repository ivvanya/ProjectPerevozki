using System;

namespace ProjectPerevozki.Models
{
    public class Driver
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Experience { get; set; }
        public DriverClass DriverClass { get; set; }
        public Driver()
        {
            Id = 0;
            DriverClass = new DriverClass();
        }
    }
}
