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

        public void Delete(int id, bool hard = false)
        {
            User userToDelete = Get(id);

            if (userToDelete == null)
                throw new ArgumentOutOfRangeException($"There are no user with id {id}");

            if (hard)
            {
                _ctx.Users.Remove(userToDelete);
            }
            else
            {
                userToDelete.IsDeleted = true;
                _ctx.Entry(userToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
        }

        public void Delete(User entity, bool hard = false)
        {
            if (hard)
            {
                _ctx.Users.Remove(entity);
            }
            else
            {
                entity.IsDeleted = true;
                _ctx.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
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

        public void Restore(int id)
        {
            User userToDelete = Get(id);

            if (userToDelete == null)
                throw new ArgumentOutOfRangeException($"There are no user with id {id}");

            userToDelete.IsDeleted = false;
            _ctx.Entry(userToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
