using Hahn.ApplicationProcess.December2020.Data.Models;
using Hahn.ApplicatonProcess.December2020.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using System.Net.Http;

namespace Hahn.ApplicationProcess.December2020.Data.Controllers
{
    /// <summary>
    /// This is the main controller of the API. It administrates the calls to the database. 
    /// </summary>


    [ApiController]
    [Route("Applicant")]
    public class ApplicantController : Controller
    {

        private static  HttpClient Client;

        ////------------Dependencies --------------// 

        private ApplicantDBContextClass _context;


        public ApplicantController(ApplicantDBContextClass context)
        {
            _context = context;
            Client = new HttpClient();
        }



        ////----------------GET----------------//

        [HttpGet]
        [Route("/GetAll")]
        public JsonResult GetAllApplicants()
        {
            try
            {
                var applicants = _context.Applicants.ToList();
                return Json(applicants);
            }
            catch (Exception)
            {

                throw;
            }
        }

        //---------------POST-------------//

        [HttpPost]
        [Route("/Add")]
        public async Task<IActionResult> AddApplicantAsync(ApplicantClass applicant)
        {
            try
            {
                var newID = _context.Applicants.Select(x => x.ID).Max() + 1;
                applicant.ID = newID;
                //_context.Applicants.Add(applicant);
                //_context.SaveChanges();
                //return StatusCode(201);


                ApplicantClassValidator validator = new ApplicantClassValidator(Client);
                ValidationResult result = await validator.ValidateAsync(applicant);
                if (result.IsValid)
                {
                    _context.Applicants.Add(applicant);
                    _context.SaveChanges();
                    return StatusCode(201);
                }
                else
                {
                    foreach (var failure in result.Errors)
                    {
                        Console.WriteLine("Property" + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                    }
                    return StatusCode(501);
                }

            }
            catch (Exception)
            {

                throw;
            }

        }


        //---------------Edit---------------//
        
        [HttpGet]
        [Route("/GetOne")]
        public IActionResult getThisOne(int ID)
        {
            try
            {
                var applicant = _context.Applicants.Find(ID);
                return Json(applicant);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut]
        [Route("/Update")]

        public IActionResult updateApplicant(ApplicantClass applicant)
        {
            try
            {
                _context.Applicants.Update(applicant);
                _context.SaveChanges();
                return StatusCode(201);
            }
            catch (Exception)
            {
                throw;
            }

        }


        //---------------DELETE--------------// 

        [HttpDelete]
        [Route("/Delete")]
        public IActionResult deleteApplicantByID(int ID)
        {
            try
            {
                var applicant = _context.Applicants.Find(ID);
                _context.Applicants.Remove(applicant);
                _context.SaveChanges();
                return StatusCode(201);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("/TryThis")]
        public async Task<bool> validateCountry(string CountryOfOrigin)
        {

            var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Head, $"https://restcountries.eu/rest/v2/name/" + CountryOfOrigin + "?fullText=true");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode && response.StatusCode.HasFlag(System.Net.HttpStatusCode.OK))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
