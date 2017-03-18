using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialSurvey.Domain.Interfaces;
using SocialSurvey.Domain.Entities;
using SocialSurvey.Domain.DB;
using System.Linq.Expressions;

namespace SocialSurvey.Domain.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private SocialSurveyContext _ctx;

        public UserRepository(SocialSurveyContext context)
        {
            _ctx = context;
        }
        public void Create(User entity)
        {
            _ctx.Users.Add(entity);
        }

        public void Delete(User entity)
        {
            entity.IsDeleted = true;
            _ctx.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public IEnumerable<User> Find(Func<User, bool> predicate)
        {
            return _ctx.Users.Where(predicate);
        }

        public User Get(int id)
        {
            return _ctx.Users.Find(id);
        }

        public IEnumerable<User> GetAll()
        {
            return _ctx.Users;
        }

        public void Restore(User entity)
        {
            entity.IsDeleted = false;
            _ctx.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void Update(User entity)
        {
            _ctx.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
    }
}
