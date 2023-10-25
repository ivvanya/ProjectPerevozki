namespace ProjectPerevozki.Models
{
    public class CargoType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CargoType()
        {
            Id = 0;
        }
    }
}