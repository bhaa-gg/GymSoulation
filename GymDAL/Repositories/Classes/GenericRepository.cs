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
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()
    {
        private readonly GymDBContext _dbContext;

        public GenericRepository(GymDBContext gymDBContext) => _dbContext = gymDBContext;


        public T? GetById(int id) => _dbContext.Set<T>().Find(id);
        public void Add(T member) =>_dbContext.Set<T>().Add(member);

        public void Delete(T entity) => _dbContext.Set<T>().Remove(entity);
            
        public void Update(T member) => _dbContext.Set<T>().Update(member);

        public IEnumerable<T> GetAll(Func<T, bool>? condition = null) =>
            condition is null ? _dbContext.Set<T>().AsNoTracking().ToList() 
                : _dbContext.Set<T>().AsNoTracking().Where(condition).ToList();

    }
}
