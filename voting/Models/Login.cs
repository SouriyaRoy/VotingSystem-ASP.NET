using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace voting.Models
{
    public class Login
    {
        [Required(ErrorMessage = "This field is required")]
        [MinLength(4)]
        [MaxLength(15)]
        [DisplayName("Username")]
        [RegularExpression("^[0-9][a-zA-Z]*$", ErrorMessage = "Do not use symbols")]
        public string UserName { get; set; }





        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{6,}$", ErrorMessage = "Password must contain: Minimum 8 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}