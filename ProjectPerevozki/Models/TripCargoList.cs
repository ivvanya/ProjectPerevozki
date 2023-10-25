namespace ProjectPerevozki.Models
{
    public class TripCargoList
    {
        public int Id { get; set; }
        public CargoType CargoType { get; set; }
        public Trip Trip { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public int Weight { get; set; }
        public int Insurance { get; set; }
        public TripCargoList()
        {
            Id = 0;
            CargoType = new CargoType();
            Trip = new Trip();
        }
    }
}
