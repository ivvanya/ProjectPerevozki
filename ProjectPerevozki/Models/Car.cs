using System;

namespace ProjectPerevozki.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int CapLoad { get; set; }
        public DateTime DateOfIssue { get; set; }
        public DateTime DateOfTo { get; set; }
        public int Mileage { get; set; }
        public byte[] Picture { get; set; }

        public Car()
        {
            Id = 0;
        }
    }
}