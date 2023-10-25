namespace ProjectPerevozki.Models
{
    public class Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HeadName { get; set; }
        public string Number { get; set; }
        public string Address { get; set; }
        public string Bank { get; set; }
        public string BankAccount { get; set; }
        public string INN { get; set; }

        public Entity()
        {
            Id = 0;
            INN = "0000000000";
        }
    }
}
