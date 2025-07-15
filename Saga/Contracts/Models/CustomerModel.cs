namespace Contracts.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }


        public ICollection<OrderModel> Orders { get; set; }
    }
}
