using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcomPainting.Models
{
    public enum ItemType
    {
        Handmade,
        Oil,
        Printed
    }
    public enum Material
    {
        Canvas,
        Paper,
        Cloth
    }
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [DisplayName("Name")]
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
        [DisplayName("Description")]
        [StringLength(200)]
        [Required]
        public string Description { get; set; }
        [StringLength(200)]
        [Required]
        public string Url { get; set; }
        [DisplayName("Price")]
        [Required]
        public int Price { get; set; }
        [DisplayName("Material")]
        [Required]
        public Material Material { get; set; }
        [DisplayName("Type")]
        [Required]
        public ItemType Type { get; set; }
    }
}
