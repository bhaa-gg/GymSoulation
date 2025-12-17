using GymDAL.Data.Contexts;
using GymDAL.Entities;
using GymDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymDAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork //, IDisposable
    {
        private readonly Dictionary<Type, object> _repositories = new();
        private readonly GymDBContext _gymDBContext;

        public UnitOfWork(GymDBContext gymDBContext, ISessionRepository sessionRepository)
        {
            _gymDBContext = gymDBContext;
            SessionRepository = sessionRepository;
        }

        public ISessionRepository SessionRepository { get; }


        #region  CLR will handle it automatically

        //public void Dispose()
        //{
        //    _gymDBContext.Dispose();         
        //} 
        #endregion

        public IGenericRepository<IEntitiy> GetRepository<IEntitiy>() where IEntitiy : BaseEntity, new()
        {
            var EntityType = typeof(IEntitiy);

            if (_repositories.TryGetValue(EntityType , out  var repo)) return (IGenericRepository<IEntitiy>)repo;


            var newRepo = new GenericRepository<IEntitiy>(_gymDBContext);
            _repositories.Add(EntityType , newRepo);
            return newRepo;

        }

        public int SaveChanges() => _gymDBContext.SaveChanges();
    }
}
