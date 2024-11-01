using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class ProductCart
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public bool? OrderConfirmed { get; set; }
    }
}
