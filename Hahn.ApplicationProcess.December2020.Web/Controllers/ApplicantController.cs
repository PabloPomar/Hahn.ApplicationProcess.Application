using Hahn.ApplicationProcess.December2020.Data.Models;
using Hahn.ApplicatonProcess.December2020.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.December2020.Data.Controllers
{
    public class ApplicantController : Controller
    {

        ////[HttpPost]
        ////[ValidateAntiForgeryToken]


        ////------------Dependencies --------------// 

        private ApplicantDBContextClass _context;


        public ApplicantController(ApplicantDBContextClass context)
        {
            _context = context;
        }



        ////----------------GET----------------//

        [HttpGet]
        [ValidateAntiForgeryToken]

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

        public IActionResult AddApplicant(ApplicantClass applicant)
        {
            try
            {
                var newID = _context.Applicants.Select(x => x.ID).Max() + 1;
                applicant.ID = newID;

                _context.Applicants.Add(applicant);
                _context.SaveChanges();
                return StatusCode(201);
            }
            catch (Exception)
            {

                throw;
            }

        }


        //---------------Edit---------------//
        
        [HttpGet]
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]

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
        [ValidateAntiForgeryToken]
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



    }
}
