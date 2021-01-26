using Hahn.ApplicationProcess.December2020.Web.Controllers;
using Hahn.ApplicationProcess.December2020.Data.Models;
using Hahn.ApplicationProcess.December2020.Web;
using Hahn.ApplicatonProcess.December2020.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace Hahn.ApplicationProcess.December2020.Tests
{
    public class Tests
    {

        private ApplicantDBContextClass _context;

        private ApplicantController _controller;

        private IStringLocalizer<ApplicantController> localizer;

        private IStringLocalizer<ApplicantClass> localizer2;

        private IHttpClientFactory factory;

        ApplicantClass FineOne;

        ApplicantClass FineOne2;

        ApplicantClass WrongOne;

        HttpClient _Client;

       [SetUp]
        public void Setup()
        {
            //Arrange
            _Client = new HttpClient();
            var _mockLocal = new Mock<IStringLocalizer<ApplicantController>>();
            var _mockLocal2 = new Mock<IStringLocalizer<ApplicantClass>>();
            string key = "TESTPASSED";
            var localizedString = new LocalizedString(key, key);
            _mockLocal.Setup(_ => _[key]).Returns(localizedString);
            _mockLocal2.Setup(_ => _[key]).Returns(localizedString);
            var _mockFactory = new Mock<IHttpClientFactory>();
            localizer = _mockLocal.Object;
            localizer2 = _mockLocal2.Object;
            factory = _mockFactory.Object;
            var options = new DbContextOptionsBuilder<ApplicantDBContextClass>()
            .UseInMemoryDatabase(databaseName: "Applicants")
            .Options;

            var _logger = new LoggerFactory().CreateLogger<ApplicantController>();

            _context = new ApplicantDBContextClass(options);

            if (_context.Applicants.Any())
            {
                return; //There was alredy info 
            }

            _context.Applicants.AddRange(
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
                    ID = 3,
                    Name = "Harry",
                    FamilyName = "Dubois",
                    Address = "Ravashiol 789",
                    CountryOfOrigin = "Canada",
                    EMailAdress = "HarHarry@yahoo.com",
                    Age = 42,
                    Hired = false

                });

            _context.SaveChanges();




            _controller = new ApplicantController(_context, _logger, localizer, localizer2, factory);

            //_controller = new ApplicantController(_context, _logger, localizer, localizer2, _Client);

            _controller.AddHttpClientForTesting(_Client);

            FineOne = new ApplicantClass(3, "Rhapael", "Du Costeau", "DiscoStreet 1487", "Argentina", "RaphaCos@gmail.com", 44, true);

            FineOne2 = new ApplicantClass(4, "Harry", "Du Costeau", "DiscoStreet 1487", "Argentina", "RaphaCos@gmail.com", 44, true);

            WrongOne = new ApplicantClass(3, "Bad", "Bad", "Bad", "FakeCountry", "bar", 1, false);

        }

        //Given that this model actualy worked building
        //When i chech if the Database has been created
        //Then the database should not be null

        [Test]
        public async System.Threading.Tasks.Task TestDatabaseCreatedAsync()
        {
            //Act
            var result = await _context.Database.EnsureCreatedAsync();
            //Assert
            Assert.IsNotNull(result);
        }


        //Given that this model build correctely
        //When i chech if the _controller exist
        //Then the controller should not be null.

        [Test]
        public void TestControllerCreated()
        {
            //Assert
            Assert.IsNotNull(_controller);
        }


        //Given that this model build correctely
        //When i ask for the applicants in the database
        //Then i should find Jhon doe and Lana Doroty in the response
        [Test]
        public void TestGetAllApplicants()
        {
            //Act
            var result = _controller.GetAllApplicants();

            var resultSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(result, Formatting.Indented);

            //Assert
            if (resultSerialized.Contains("Jhon") && resultSerialized.Contains("Doe") && resultSerialized.Contains("Lana") && resultSerialized.Contains("Doroty"))
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
        }


        //Given that this model build correctely
        //When i ask for the applicants with ID 1
        //Then i should find Jhon doe in the response
        [Test]
        public void TestGetOneApplicants()
        {
            //Act
            var result = _controller.getThisOne(1);

            var resultSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(result, Formatting.Indented);

            //Assert
            if (resultSerialized.Contains("Jhon") && resultSerialized.Contains("Doe"))
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
        }


        //Given that this model build correctely and the database entry its not being tracked
        // (Also for some reason i dont understand this test must be first or it fails, didn't have time to find why)
        //When i put the "Fine One"  applicant model on the updateApplicantAsync of controller
        //Then i should receibe an 201 status code
        [Test, Order(1)]
        public async System.Threading.Tasks.Task TestUpdateOneApplicantsAsync()
        {

            //Arrange

            var local = _context.Applicants
                            .FirstOrDefault(entry => entry.ID.Equals(3));

            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;

            }

            //Act

            var result = await _controller.updateApplicantAsync(FineOne);

            //Assert
            var resultSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(result, Formatting.Indented);

            if (resultSerialized.Contains("\"StatusCode\": 201"))
            {

                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
        }

        //Given that this model build correctely and the database entry its not being tracked
        //When i put the "wrong One" applicant model on the updateApplicantAsync of controller
        //Then i should receibe an 400 status code
        [Test]
        public async System.Threading.Tasks.Task TestFailUpdateOneApplicantsAsync()
        {

            //Arrange

            var local = _context.Applicants
                            .FirstOrDefault(entry => entry.ID.Equals(3));

            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }

            //Act

            var result = await _controller.updateApplicantAsync(WrongOne);

            var resultSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(result, Formatting.Indented);


            //Assert
            if (resultSerialized.Contains("\"StatusCode\": 400"))
            {

                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
        }

        //Given that this model build correctely
        //When i put the "wrong One" applicant model and ask for the AddApplicantAsync method in the controller
        //Then i should receibe an 400 status code

        [Test]
        public async System.Threading.Tasks.Task TestFailAddOneApplicantsAsync()
        {

            //Act

            var result = await _controller.AddApplicantAsync(WrongOne);

            var resultSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(result, Formatting.Indented);

            //Assert

            if (resultSerialized.Contains("\"StatusCode\": 400"))
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
        }

        //Given that this model build correctely
        //When i put the "FineOne" applicant model and ask for the AddApplicantAsync method in the controller
        //Then i should receibe an 201 status code

        [Test]
        public async System.Threading.Tasks.Task TestAddOneApplicantsAsync()
        {

            //Act

            var result = await _controller.AddApplicantAsync(FineOne);

            var resultSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(result, Formatting.Indented);

            //Assert

            if (resultSerialized.Contains("\"StatusCode\": 201"))
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
        }


        //Given that this model build correctely
        //When i put the ID 3 and ask for the deleteApplicantByID method in the controller
        //Then i should receibe an 201 status code
        [Test]
        public void TestDeleteOneApplicantsAsync()
        {

            //Act

            var result =  _controller.deleteApplicantByID(3);

            var resultSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(result, Formatting.Indented);

            //Assert

            if (resultSerialized.Contains("\"StatusCode\": 201"))
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
        }


        //Given that this model build correctely
        //When i ask to the WebApiHelperClass if Argentina exist
        //Then i should receibe a true
        [Test]
        public async System.Threading.Tasks.Task ValidateCountryAsync()
        {

            //Arrange

            ApplicantClassValidator validator = new ApplicantClassValidator(_Client, localizer2);
            WebApiHelperClass helper = new WebApiHelperClass(_Client);

            //Act

            var result = await helper.validateCountry("Argentina");

            //Assert

            Assert.IsTrue(result);
        }

        //Given that this model build correctely
        //When i ask to the WebApiHelperClass if FakeCountry exist
        //Then i should receibe a false
        [Test]
        public async System.Threading.Tasks.Task ValidateCountryFailAsync()
        {

            //Arrange

            ApplicantClassValidator validator = new ApplicantClassValidator(_Client, localizer2);
            WebApiHelperClass helper = new WebApiHelperClass(_Client);

            //Act

            var result = await helper.validateCountry("FakeCountry");

            //Assert

            Assert.IsFalse(result);
        }

        //Given that this model build correctely
        //When if the Fine one application model is valid
        //Then i should receibe that result Is Valid

        [Test]
        public void ValidateValidModel()
        {

            //Arrange

            ApplicantClassValidator validator = new ApplicantClassValidator(_Client, localizer2);

            //Act

            var result = validator.Validate(FineOne);

            //Assert

            Assert.IsTrue(result.IsValid);
        }

        //Given that this model build correctely
        //When if the Wrong One application model is valid
        //Then i should receibe that result Is not Valid
        [Test]
        public void ValidateInvalidModel()
        {

            //Arrange

            ApplicantClassValidator validator = new ApplicantClassValidator(_Client, localizer2);

            //Act

            var result = validator.Validate(WrongOne);

            //Assert

            Assert.IsFalse(result.IsValid);
        }

    }
}