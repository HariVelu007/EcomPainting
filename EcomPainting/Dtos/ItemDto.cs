using EcomPainting.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EcomPainting.Dtos
{
    public class ItemFilter
    {
        [DisplayName("Canvas")]
        public bool MaterialCanvas { get; set; }= true;
        [DisplayName("Paper")]
        public bool MaterialPaper { get; set; } = true;
        [DisplayName("Cloth")]
        public bool MaterialCloth { get; set; } = true;
        [DisplayName("Handmade")]
        public bool TypeHandmade { get; set; } = true;
        [DisplayName("Oil")]
        public bool TypeOil { get; set; } = true;
        [DisplayName("TPrinted")]
        public bool TypePrinted { get; set; } = true;
        [DisplayName("From")]
        public int PriceFrom { get; set; }
        [DisplayName("To")]
        public int PriceTo { get; set; }
        [DisplayName("Landscape")]
        public bool OrientationLandscape { get; set; } = true;
        [DisplayName("Protrait")]
        public bool OrientationProtrait { get; set; } = true;
        public int PageNo { get; set; } = 1;
    }

    public class ItemList
    {
        public List<Item> Items { get; set; }
        public int PageNo { get; set; } = 1;
        public int TotalRecords { get; set; } = 0;

    }
    public class ItemPageDto
    {
        public ItemFilter filter { get; set; }
        public ItemList itemList { get; set; }
    }
    public class ItemDto
    {      
        public int Id { get; set; }
        [DisplayName("Name")]
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
        [DisplayName("Description")]
        [StringLength(200)]
        [Required]
        public string Description { get; set; }
        [StringLength(200)]        
        public string Url { get; set; }="~/images/upload.png";
        [DisplayName("Price")]
        [Required]
        public int Price { get; set; }
        [DisplayName("Material")]
        [Required]
        public Material Material { get; set; }
        [DisplayName("Type")]
        [Required]
        public ItemType Type { get; set; }
        
        [DisplayName("Item Image")]
        public IFormFile ItemImage { get; set; }
    }
}
