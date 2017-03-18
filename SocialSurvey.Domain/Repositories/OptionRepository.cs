using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialSurvey.Domain.Interfaces;
using SocialSurvey.Domain.Entities;
using SocialSurvey.Domain.DB;

namespace SocialSurvey.Domain.Repositories
{
    public class OptionRepository : IRepository<Option>
    {
        SocialSurveyContext _ctx;

        public OptionRepository(SocialSurveyContext context)
        {
            _ctx = context;
        }

        public void Create(Option entity)
        {
            _ctx.Options.Add(entity);
        }

        public void Delete(int id)
        {
            Option optionToDelete = Get(id);
            if (optionToDelete == null)
                throw new ArgumentOutOfRangeException($"There are no option with id - {id}");
            optionToDelete.IsDeleted = true;
            _ctx.Entry(optionToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void Delete(Option entity)
        {
            entity.IsDeleted = true;
            _ctx.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public IEnumerable<Option> Find(Func<Option, bool> predicate)
        {
            return _ctx.Options.Where(predicate);
        }

        public Option Get(int id)
        {
            return _ctx.Options.Find(id);
        }

        public IEnumerable<Option> GetAll()
        {
            return _ctx.Options;
        }

        public void Restore(int id)
        {
            Option optionToRestore = Get(id);
            if (optionToRestore == null)
                throw new ArgumentOutOfRangeException($"There are no option with id - {id}");

            optionToRestore.IsDeleted = false;
            _ctx.Entry(optionToRestore).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void Restore(Option entity)
        {
            entity.IsDeleted = false;
            _ctx.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void Update(Option entity)
        {
            _ctx.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
    }
}
