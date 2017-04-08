using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialSurvey.Domain.Entities
{
    public enum Role
    {
        Interviewer,
        Controller,
        Admin
    }
    public class User
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public Role Role { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsDeleted { get; set; }


        public List<Survey> Surveys { get; set; }
        public List<Form> Forms { get; set; }
        public User()
        {
            Surveys = new List<Survey>();
            Forms = new List<Form>();
        }
    }
}
