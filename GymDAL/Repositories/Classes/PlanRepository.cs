using GymDAL.Data.Contexts;
using GymDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymDAL.Repositories.Interfaces
{
    public class PlanRepository : IPlanRepository
    {

        private readonly GymDBContext _dbContext;

        public PlanRepository(GymDBContext gymDBContext ) => _dbContext = gymDBContext;

        public IEnumerable<Plan> GetAll() => _dbContext.Plans.ToList() ;

        public Plan? GetById(int id)=> _dbContext.Plans.Find(id) ;

        public int Update(Plan plan)
        {
            _dbContext.Plans.Update(plan);
            return _dbContext.SaveChanges();
        }
    }
}
