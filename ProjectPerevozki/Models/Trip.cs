using System;

namespace ProjectPerevozki.Models
{
    public class Trip
    {
        public int Id { get; set; }
        public Contract Contract { get; set; }
        public Car Car { get; set; }
        public DateTime InputTime { get; set; }
        public Trip()
        {
            Id = 0;
            Contract = new Contract();
            Car = new Car();
        }
    }
}
