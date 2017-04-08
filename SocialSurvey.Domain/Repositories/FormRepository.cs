using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialSurvey.Domain.Interfaces;
using SocialSurvey.Domain.Entities;
using SocialSurvey.Domain.DB;

namespace SocialSurvey.Domain.Repositories
{
    public class FormRepository : IRepository<Form>
    {
        private SocialSurveyContext _ctx;

        public FormRepository(SocialSurveyContext context)
        {
            _ctx = context;
        }

        public void Create(Form entity)
        {
            _ctx.Add(entity);
        }

        public void Delete(int id, bool hard = true)
        {
            throw new NotImplementedException();
        }

        public void Delete(Form entity, bool hard = true)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Form> Find(Func<Form, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Form Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Form> GetAll()
        {
            return _ctx.Forms;
        }

        public void Restore(int id)
        {
            throw new NotImplementedException();
        }

        public void Restore(Form entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Form entity)
        {
            throw new NotImplementedException();
        }
    }
}
