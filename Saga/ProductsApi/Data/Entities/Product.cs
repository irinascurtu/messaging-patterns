using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsApi.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string LongDescription { get; set; }
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
    }
}
