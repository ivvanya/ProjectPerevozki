using System;

namespace ProjectPerevozki.Models
{
    public class Contract
    {
        public int Id { get; set; }
        public Employee Employee { get; set; }
        public DateTime OrderDate { get; set; }
        public Client Recipient { get; set; }
        public string RecipientAddress { get; set; }
        public Client Sender { get; set; }
        public string SenderAddress { get; set; }
        public int RouteLength { get; set; }
        public int Price { get; set; }
        public Contract()
        {
            Id = 0;
            Employee = new Employee();
            Recipient = new Client();
            Sender = new Client();
        }
    }
}
