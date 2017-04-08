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

        public void Delete(int id, bool hard = false)
        {
            var optionToDelete = _ctx.Options.Find(id);

            if (hard)
            {
                _ctx.Options.Remove(optionToDelete);
            }
            else
            {
                optionToDelete.IsDeleted = true;
                _ctx.Entry(optionToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
        }

        public void Delete(Option entity, bool hard = false)
        {
            var optionToDelete = _ctx.Options.Find(entity.OptionId);

            if (hard)
            {
                _ctx.Options.Remove(optionToDelete);
            }
            else
            {
                optionToDelete.IsDeleted = true;
                _ctx.Entry(optionToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
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
            Option optionToRestore = _ctx.Options.Find(id);
            if (optionToRestore == null)
                throw new ArgumentException($"There are no option with id - {id}");

            optionToRestore.IsDeleted = false;
            _ctx.Entry(optionToRestore).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void Restore(Option entity)
        {
            var optionToRestore = _ctx.Options.Find(entity.OptionId);
            if (optionToRestore == null)
                throw new ArgumentException($"There is no question with id - {entity.OptionId}");

            optionToRestore.IsDeleted = false;
            _ctx.Entry(optionToRestore).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void Update(Option entity)
        {
            var existingOption = _ctx.Options
                .Where(o => o.OptionId == entity.OptionId)
                .SingleOrDefault();

            if (existingOption == null)
                throw new ArgumentException($"There is no option with id {entity.OptionId}");

            //Update option
            _ctx.Entry(existingOption).CurrentValues.SetValues(entity);
        }
    }
}
