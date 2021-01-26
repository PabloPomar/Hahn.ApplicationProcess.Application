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
using Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Filters;

namespace Hahn.ApplicationProcess.December2020.Web.Controllers
{
    /// <summary>
    /// This is the main controller of the API. It administrates the calls to the database. 
    /// </summary>
    [ApiController]
    [Route("Applicant")]
    public class ApplicantController : Controller
    {

        private static HttpClient Client;

        private readonly IHttpClientFactory _clientFactory;

        private readonly IStringLocalizer<ApplicantController> _localizer;

        private readonly IStringLocalizer<ApplicantClass> _localizer2;

        ////------------Dependencies --------------// 

        private ApplicantDBContextClass _context;

        /// <summary>
        /// This initiates the controller.
        /// </summary>
        public ApplicantController(ApplicantDBContextClass context, ILogger<ApplicantController> logger, IStringLocalizer<ApplicantController> localizer, IStringLocalizer<ApplicantClass> localizer2, IHttpClientFactory clientFactory)
        {
            _context = context;
            _localizer = localizer;
            _localizer2 = localizer2;
            _clientFactory = clientFactory;
            Client = _clientFactory.CreateClient("appClient");
        }



        ////----------------GET----------------//

        /// <summary>
        /// This gets all current applicants in the database.
        /// </summary>
        /// <response code="200">Returns all the applicant in the base</response>
        /// <response code="400">Something went grong with the client</response>     
        [HttpGet]
        [Route("/GetAll")]
        public IActionResult GetAllApplicants()
        {
            try
            {
                var applicants = Json(_context.Applicants.ToList());
                return (applicants);
            }
            catch (Exception Ex)
            {
                string errmessage = string.Format(_localizer["Error trying to get all the applicants:"] + "{0}", Ex);
                Log.Error(errmessage);
                return StatusCode(400, Ex);
            }
        }

        //---------------POST-------------//


        /// <summary>
        /// Add One Applicant.
        /// </summary>
        /// <remarks>
        /// Note: In this case the id param its unnecessary, it will be ignored in the post request.
        /// </remarks>
        /// <response code="201">Returns the newly added applicant id</response>
        /// <response code="400">If the model doesn't pass validation</response>        
        /// <param name="applicant"></param>
        [HttpPost]
        [Route("/Add")]
        public async Task<IActionResult> AddApplicantAsync(ApplicantClass applicant)
        {
            var errors = "";
            try
            {
                var newID = _context.Applicants.Select(x => x.ID).Max() + 1;
                applicant.ID = newID;
                ApplicantClassValidator validator = new ApplicantClassValidator(Client, _localizer2);
                ValidationResult result = await validator.ValidateAsync(applicant);
                if (result.IsValid)
                {
                    _context.Applicants.Add(applicant);
                    _context.SaveChanges();
                    Log.Information(_localizer["New Applicant added."]);
                    String findItAt = _localizer["The new applicant can be seen in:"] + "https://localhost:5001/GetOne?ID=" + newID;
                    return StatusCode(201, findItAt);
                }
                else
                {
                    foreach (var failure in result.Errors)
                    {
                        errors += _localizer["Property"] + " " + _localizer[failure.PropertyName] + " " + _localizer["failed validation. Error was:"] + failure.ErrorMessage + Environment.NewLine;
                    }
                    string errmessage = string.Format(_localizer["Error trying to add model (Validation error):"] + "{0}.", errors);
                    Log.Error(errmessage);
                    return StatusCode(400, errors);
                }

            }
            catch (Exception Ex)
            {
                Log.Error(_localizer["Error trying to add model:"] + " {Ex}.", Ex);
                return Json(StatusCode(400, Ex));
            }

        }


        //---------------Edit---------------//

        /// <summary>
        /// Get Applicant By Id
        /// </summary>
        /// <response code="200">Returns the applicant with the requested ID</response>
        /// <response code="400">Something went grong with the client or there was no applicant with that ID</response>    
        [HttpGet]
        [Route("/GetOne")]
        public IActionResult getThisOne(int ID)
        {
            try
            {
                var applicant = _context.Applicants.Find(ID);           
                return Json(applicant);
            }
            catch (Exception Ex)
            {
                string errmessage = string.Format(_localizer["Error trying to get this applicant:"] + " {0}.", Ex);
                Log.Error(errmessage);
                return StatusCode(400, Ex);
            }
        }

        /// <summary>
        /// Updates an applicant.
        /// </summary>
        /// <response code="200">The Applicant was succesfully updated</response>
        /// <response code="400">The new params could not be validated or there was no applicant with that ID</response>    
        [HttpPut]
        [Route("/Update")]
        public async Task<IActionResult> updateApplicantAsync(ApplicantClass applicant)
        {
            var errors = "";
            try
            {
                ApplicantClassValidator validator = new ApplicantClassValidator(Client, _localizer2);
                ValidationResult result = await validator.ValidateAsync(applicant);
                if (result.IsValid)
                {
                    _context.Applicants.Update(applicant);
                    _context.SaveChanges();
                    string message = string.Format(_localizer["Applicant"] +" {0} " +  _localizer["updated."], applicant.ID);
                    Log.Information(message);
                    var Url = "https://localhost:5001/GetOne?ID=" + applicant.ID;
                    return StatusCode(201, Url);
                }
                else
                {
                    foreach (var failure in result.Errors)
                    {
                        errors += "Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage  + Environment.NewLine;                     
                    }
                    string errmessage = string.Format(_localizer["Error trying to update model (Validation error) :"] + " {0}.", errors);
                    Log.Error(errmessage);
                    return StatusCode(400, errors);
                }
            }
            catch (Exception Ex)
            {
                string errmessage = string.Format(_localizer["Error trying to update this applicant:"] + " {0}.", Ex);
                Log.Error(errmessage);
                return StatusCode(400, Ex);
            }

        }


        //---------------DELETE--------------// 

        /// <summary>
        /// Deletes an applicant.
        /// </summary>
        /// <response code="200">The Applicant was succesfully deleted</response>
        /// <response code="400">Something went wrong with the client or there was no applicant with that ID</response>    
        [HttpDelete]
        [Route("/Delete")]
        public IActionResult deleteApplicantByID(int ID)
        {
            try
            {
                var applicant = _context.Applicants.Find(ID);
                _context.Applicants.Remove(applicant);
                _context.SaveChanges();
                string message = string.Format(_localizer["Applicant"] + " {0} " + _localizer["deleted."], applicant.ID);
                message = _localizer[message];
                Log.Warning(message);
                return StatusCode(201);
            }
            catch (Exception Ex)
            {
                string errmessage = string.Format(_localizer["Error trying to delete this applicant:"] + " {0} ", Ex);
                errmessage = _localizer[errmessage];
                Log.Error(errmessage);
                return StatusCode(400, Ex);
            }
        }

        //[HttpGet]
        //[Route("/TryThis")]
        //public async Task<bool> validateCountry(string CountryOfOrigin)
        //{

        //    var client = new HttpClient();

        //    var request = new HttpRequestMessage(HttpMethod.Head, $"https://restcountries.eu/rest/v2/name/" + CountryOfOrigin + "?fullText=true");

        //    var response = await client.SendAsync(request);

        //    if (response.IsSuccessStatusCode && response.StatusCode.HasFlag(System.Net.HttpStatusCode.OK))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}


        /// <summary>
        /// This is for testing
        /// </summary>
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public void AddHttpClientForTesting(HttpClient clie)
        {
            Client = clie;
        }


    }
}
