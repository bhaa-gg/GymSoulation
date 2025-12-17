using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymBLL.ViewModels.PlanViewModels
{
    public class UpdatePlanViewModel
    {

        //[Required(ErrorMessage = "Plan Name Is Required")]
        //[StringLength(50,ErrorMessage = "Plan Name Must Be Less Than 51 Char")]
        public string PlanName { get; set; } = null!;


        [Required(ErrorMessage = "Description Name Is Required")]
        [StringLength(50,ErrorMessage = "Description Must Be Between 0 and 200 Char")]
        public string Description { get; set; } = null!;


        [Required(ErrorMessage = "Price Name Is Required")]
        [Range(0.1  , 10000, ErrorMessage = "Price Must Be Between 0.1 and 10000 Char")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "DurationDays Name Is Required")]
        [Range(1  , 365, ErrorMessage = "DurationDays Must Be Between 1 and 365 Char")]
        public int DurationDays { get; set; }



    }
}
