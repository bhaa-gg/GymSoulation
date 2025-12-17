using GymDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymBLL.ViewModels.MemberViewModels
{
    internal class UpdateMemberViewModel
    {

        public string Name { get; set; } = null!;

        public string? Photo { get; set; }




        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]//validation
        [DataType(DataType.EmailAddress)]//ui hint
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email Must be Between 5 and 100 Chars")]
        public string Email { get; set; } = null!;


        [Required(ErrorMessage = "Phone Is Required")]
        [Phone(ErrorMessage = "Invalid Phone Format")] //validation
        [DataType(DataType.PhoneNumber)] //ui hint
        [RegularExpression(@"^(015|010|011|012)\d{8}$", ErrorMessage = "Phone number Must Be Valid Egyptian Phone Number")]
        public string Phone { get; set; } = null!;


        

        #region Address
        [Required(ErrorMessage = "Building Number Is Required")]
        [Range(1, 9000, ErrorMessage = "Building Number  Must Be Between 1 , 9000")]
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage = "Street Is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Street Must be Between 2 and 30 Chars")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "City Is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "City Must be Between 2 and 30 Chars")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City Can Contain Only Letters And Spaces")]
        public string City { get; set; } = null!;


        #endregion

    }
}
