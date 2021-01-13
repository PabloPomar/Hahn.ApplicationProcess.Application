using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.December2020.Domain.Models
{
    public class ApplicantClass
    {

        private int _ID;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }


        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _FamilyName;

        public string FamilyName
        {
            get { return _FamilyName; }
            set { _FamilyName = value; }
        }


        private string _Address;

        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }


        private string _CountryOfOrigin;

        public string CountryOfOrigin
        {
            get { return _CountryOfOrigin; }
            set { _CountryOfOrigin = value; }
        }

        private string _EMailAdress;

        public string EMailAdress
        {
            get { return _EMailAdress; }
            set { _EMailAdress = value; }
        }

        private int _Age;

        public int Age
        {
            get { return _Age; }
            set { _Age = value; }
        }

        private bool _Hired;

        public bool Hired
        {
            get { return _Hired; }
            set { _Hired = value; }
        }


    }
}
