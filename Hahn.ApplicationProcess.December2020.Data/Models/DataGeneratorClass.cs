using Hahn.ApplicatonProcess.December2020.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.December2020.Data.Models
{
    public class DataGeneratorClass
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicantDBContextClass(serviceProvider.GetRequiredService<DbContextOptions<ApplicantDBContextClass>>());
            if (context.Applicants.Any())
            {
                return; //There was alredy info 
            }

            context.Applicants.AddRange(
                new ApplicantClass
                {
                    ID = 1,
                    Name = "Jhon",
                    FamilyName = "Doe",
                    Address = "Fake street 123",
                    CountryOfOrigin = "Peru",
                    EMailAdress = "DoeJhon@gmail.com",
                    Age = 24,
                    Hired = true

                },

                new ApplicantClass
                {
                    ID = 2,
                    Name = "Lana",
                    FamilyName = "Doroty",
                    Address = "Douglas street 231",
                    CountryOfOrigin = "England",
                    EMailAdress = "DLana45@gmail.com",
                    Age = 35,
                    Hired = true

                },

                new ApplicantClass
                {
                    ID = 2,
                    Name = "Harry",
                    FamilyName = "Dubois",
                    Address = "Ravashiol 789",
                    CountryOfOrigin = "Canada",
                    EMailAdress = "HarHarry@yahoo.com",
                    Age = 42,
                    Hired = false

                });

            context.SaveChanges();
        }
    }
}
