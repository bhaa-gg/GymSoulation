using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymDAL.Entities
{
    public abstract class   GymUser : BaseEntity
    {

        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateOnly DOB { get; set; }

        public string Gender { get; set; }= null!;


        public Adress Adress { get; set; } = null!;

    }
}
