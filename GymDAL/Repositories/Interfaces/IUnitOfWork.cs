using GymDAL.Entities;
using GymDAL.Repositories.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymDAL.Repositories.Interfaces
{
    public interface IUnitOfWork 
    {
        public ISessionRepository SessionRepository { get; }
        IGenericRepository<IEntitiy> GetRepository<IEntitiy>() where IEntitiy : BaseEntity , new();

        int SaveChanges();
    }
}
