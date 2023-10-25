using System;

namespace ProjectPerevozki.Models
{
    public class Individual
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string PassSeries { get; set; }
        public string PassNumber { get; set; }
        public DateTime DateOfIssue { get; set; }
        public string IssuedBy { get; set; }

        public Individual()
        {
            Id = 0;
            PassSeries = "0000";
            PassNumber = "000000";
        }
    }
}
