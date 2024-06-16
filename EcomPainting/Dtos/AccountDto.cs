using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EcomPainting.Dtos
{
    public class AccountDto
    {
    }
    public class LoginDto
    {
        [StringLength(50)]
        [Required]
        [EmailAddress]
        [DisplayName("User Mail")]
        public string UserMail { get; set; }

        [StringLength(50)]
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }
    }
    public class UserDto
    {
        [StringLength(50)]
        [Required]
        [EmailAddress]
        [DisplayName("User Mail")]
        public string UserMail { get; set; }

        [StringLength(50)]
        [Required]
        [DisplayName("UserName")]
        public string UserName { get; set; }

        [StringLength(50)]
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [StringLength(50)]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
