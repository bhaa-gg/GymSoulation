using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymBLL.ViewModels.SessionViewModels
{
    public class SessionViewModel
    {
        public int Id { get; set; }
        public string CategoryName{ get; set; } = null!;
        public string Description{ get; set; } = null!;
        public string TrainerName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int Capacity{ get; set; }
        public int AvailableSlots { get; set; }



        #region ComputedProps

        public string DateDisplay => $"{StartDate:MMM dd ,yyy}";
        public string TimeDisplay => $"{StartDate:hh:mm tt} - {EndDate:hh:mm tt}";
        public TimeSpan Duration =>EndDate - StartDate;

        public string Status{ get=>StartDate > DateTime.Now ? "Upcoming" :
                StartDate <= DateTime.Now && EndDate >= DateTime.Now ?"Ongoing":"Completed" ; }

        #endregion

    }
}
