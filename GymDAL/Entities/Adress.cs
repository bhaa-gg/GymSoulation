using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymDAL.Entities
{
    [Owned]
    public class Adress
    {
        public int BuildingNymber { get; set; }
        public string Street { get; set; } = null!;
        public string City{ get; set; } = null!;
    }
}
