namespace ProjectPerevozki.Models
{
    public class TripDriverList
    {
        public Trip Trip { get; set; }
        public Driver Driver { get; set; }
        public TripDriverList()
        {
            Driver = new Driver();
            Trip = new Trip();
        }
    }
}
