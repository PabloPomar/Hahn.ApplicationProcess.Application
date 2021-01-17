using Hahn.ApplicatonProcess.December2020.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.December2020.Data.Models
{
    public class ApplicantDBContextClass : DbContext
    {

        public ApplicantDBContextClass(DbContextOptions<ApplicantDBContextClass> options) 
            : base(options) { }


        private DbSet<ApplicantClass> _Applicants;

        public DbSet<ApplicantClass> Applicants
        {
            get { return _Applicants; }
            set { _Applicants = value; }
        }




    }
}
