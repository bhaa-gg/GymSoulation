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
    internal class MemberRepository : IMemberRepository
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

        public IEnumerable<Member> GetAll()=> _dbContext.Members.ToList();
        public Member? GetById(int id) =>   _dbContext.Members.Find(id);

        public int Update(Member member)
        {
            _dbContext.Members.Update(member);
            return _dbContext.SaveChanges();

        }
    }
}
