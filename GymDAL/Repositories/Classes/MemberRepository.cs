using GymDAL.Data.Contexts;
using GymDAL.Entities;
using GymDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GymDAL.Repositories.Classes
{
    public class MemberRepository : IMemberRepository
    {

        private readonly GymDBContext _dbContext;

        public MemberRepository( GymDBContext gymDBContext)
        {
            _dbContext = gymDBContext;
        }



        public int Add(Member member)
        {
            _dbContext.Members.Add(member);
            return _dbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            var res = _dbContext.Members.Find(id);
            if(res is null) return 0;
            _dbContext.Members.Remove(res);
            return _dbContext.SaveChanges();
        }

        public void Delete(Member entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Member> GetAll()=> _dbContext.Members.ToList();

        public IEnumerable<Member> GetAll(Func<Member, bool>? condition = null)
        {
            throw new NotImplementedException();
        }

        public Member? GetById(int id) =>   _dbContext.Members.Find(id);

        public int Update(Member member)
        {
            _dbContext.Members.Update(member);
            return _dbContext.SaveChanges();

        }

        void IGenericRepository<Member>.Add(Member member)
        {
            throw new NotImplementedException();
        }

        void IGenericRepository<Member>.Update(Member member)
        {
            throw new NotImplementedException();
        }
    }
}
