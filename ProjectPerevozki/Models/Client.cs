namespace ProjectPerevozki.Models
{
    public class Client
    {
        public int Id { get; set; }
        public bool FaceStatus { get; set; }
        public Individual Individual { get; set; }
        public Entity Entity { get; set; }
        public Client()
        {
            Entity = new Entity();
            Individual = new Individual();
        }
    }
}
