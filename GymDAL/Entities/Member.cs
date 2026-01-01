using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymDAL.Entities
{
    public class Member : GymUser
    {
        public string? Photo { get; set; } = null!;

        public  HealthRecord HealthRecord { get; set; } = null!;

        public ICollection<MemberShip> MemberShips { get; set; } = null!;
        public ICollection<MemberSession> MemberSessions { get; set; } = null!;
    }
}
