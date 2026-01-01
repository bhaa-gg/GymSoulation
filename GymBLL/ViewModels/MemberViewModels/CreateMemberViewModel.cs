using GymDAL.Entities.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
RegularExpression 
    /s=> have White space
    *=> Zero or More
    +=> one or More
*/
namespace GymBLL.ViewModels.MemberViewModels
{
    public class CreateMemberViewModel
    {


        [Required(ErrorMessage ="Photo Is Required")]
        [Display(Name="Profile Photo")]
        public IFormFile PhotoFile{ get; set; }= null!;

        #region Member
        [Required(ErrorMessage = "Name Is Required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name Must be Between 2 and 50 Chars")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name Can Contain Only Letters And Spaces")]
        public string Name { get; set; } = null!;

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


        [Required(ErrorMessage = "Gender Is Required")]
        public Gender Gender { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "DOB Is Required")]
        public DateOnly DOB { get; set; }

        #endregion

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

        #region HR

        [Required(ErrorMessage = "Health Record Is Required")]
        public HealthRecordViewModel HealthRecordViewModel { get; set; } = null!;
        #endregion


    }
}
