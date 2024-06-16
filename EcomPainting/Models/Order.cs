using System.ComponentModel.DataAnnotations.Schema;

namespace EcomPainting.Models
{
    public class Order
    {
        public int ID { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public DateTime PlacedOn { get; set; }= DateTime.Now;
    }
}
