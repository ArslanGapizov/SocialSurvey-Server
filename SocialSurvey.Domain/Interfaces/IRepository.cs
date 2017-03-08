using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSurvey.Domain.Interfaces
{
    public interface IRepository<T> where T : class 
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        IEnumerable<T> Find(Func<T, Boolean> predicate);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);

    }
}
