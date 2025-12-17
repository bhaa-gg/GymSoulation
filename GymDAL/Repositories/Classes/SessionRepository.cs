using GymDAL.Data.Contexts;
using GymDAL.Entities;
using GymDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymDAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository 
    {
        private readonly GymDBContext _gymDBContext;

        public SessionRepository(GymDBContext gymDBContext) : base(gymDBContext)
        {
            _gymDBContext = gymDBContext;
        }

      
        public IEnumerable<Session> GetAllSessionsWithTrainerAndCategory()
        {
            return  _gymDBContext.Sessions
                .Include(S => S.SessionCategory)
                .Include(S => S.SessionTrainer)
                .ToList();
        }

      
        public int GetCountOfBookedSlots(int sessionId)=>_gymDBContext.MemberSessions
            .Count(MS => MS.SessionId == sessionId);

        public Session? GetSessionByIdWithTrainerAndCategory(int sessionId)
        {
            return _gymDBContext.Sessions
              .Include(S => S.SessionCategory)
              .Include(S => S.SessionTrainer)
               .FirstOrDefault(S => S.Id == sessionId);
        }
    }
}
