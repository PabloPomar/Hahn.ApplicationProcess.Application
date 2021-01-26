using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.December2020.Domain.Models
{

    /// <summary>
    /// The model class of the APP, it represents an applicant data. 
    /// </summary>

    public class ApplicantClass
    {

        private int _ID;


        /// <summary>
        /// The ID of the Applicant
        /// </summary>
        /// <example>3</example>
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }


        private string _Name;

        /// <summary>
        /// The Name of the Applicant. Must have at least 5 characters,
        /// </summary>
        /// <example>Raphael</example>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _FamilyName;

        /// <summary>
        /// The Surname of the Applicant. Must have at least 5 characters,
        /// </summary>
        /// <example>Ambrosius Costeau</example>
        public string FamilyName
        {
            get { return _FamilyName; }
            set { _FamilyName = value; }
        }


        private string _Address;

        /// <summary>
        /// The Address of the Applicant. Must have at least 10 characters,
        /// </summary>
        /// <example>Ravashol DiscoStreet 4242</example>
        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }


        private string _CountryOfOrigin;

        /// <summary>
        /// The country of the applicant. It must Exist. 
        /// </summary>
        /// <example>Mexico</example>

        public string CountryOfOrigin
        {
            get { return _CountryOfOrigin; }
            set { _CountryOfOrigin = value; }
        }

        private string _EMailAdress;

        /// <summary>
        /// The Email of the applicant. It must be valid.
        /// </summary>
        /// <example>Racosteau@gmail.com</example>
        public string EMailAdress
        {
            get { return _EMailAdress; }
            set { _EMailAdress = value; }
        }

        private int _Age;

        /// <summary>
        /// The age of the applicant. It must be between 20 and 60. 
        /// </summary>
        /// <example>44</example>
        public int Age
        {
            get { return _Age; }
            set { _Age = value; }
        }

        private bool _Hired;

        /// <summary>
        /// If the applicant is hired or not. 
        /// </summary>
        /// <example>true</example>
        public bool Hired
        {
            get { return _Hired; }
            set { _Hired = value; }
        }

        //public bool validateModel()
        //{
        //    return false;
        //}





    }
}
