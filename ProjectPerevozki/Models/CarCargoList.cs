namespace ProjectPerevozki.Models
{
    public class CarCargoList
    {
        public Car Car { get; set; }
        public CargoType CargoType { get; set; }

        public CarCargoList()
        {
            Car = new Car();
            CargoType = new CargoType();
        }
    }
}
