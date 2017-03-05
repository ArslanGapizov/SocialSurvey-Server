﻿using System;
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
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public Role Role { get; set; }
        public string Region { get; set; }
        public string Area { get; set; }
        public string Sector { get; set; }
        public DateTime CreationDate { get; set; }

        public List<Survey> Surveys { get; set; }
        public User()
        {
            Surveys = new List<Survey>();
        }
    }
}