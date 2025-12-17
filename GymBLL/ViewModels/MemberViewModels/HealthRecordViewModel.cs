using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymBLL.ViewModels.MemberViewModels
{
    internal class HealthRecordViewModel
    {


        [Required(ErrorMessage = "Height Is Required")]
        [Range(0.1,300 , ErrorMessage = "Height Must Be Greater Than 0 and Less Than 300")]
        public decimal Height { get; set; }

        [Required(ErrorMessage = "Weight Is Required")]
        [Range(0.1,500 , ErrorMessage = "Weight Must Be Greater Than 0 and Less Than 500")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "BloodType Is Required")]
        [StringLength(3,  ErrorMessage = "BloodType Must be 3 Chars or Less")]
        public string BloodType { get; set; } = null!;

        public string? Note { get; set; }
    }
}
