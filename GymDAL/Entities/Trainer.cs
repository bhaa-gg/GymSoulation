using GymDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymDAL.Entities
{
    public class Trainer : GymUser
    {
        public Specialties Specialties { get; set; }



        public ICollection<Session> TrainerSessions { get; set; } = null!;
    }
}
