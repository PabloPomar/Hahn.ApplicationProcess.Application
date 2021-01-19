using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.December2020.Domain.Models
{
    /// <summary>
    /// This class is for making requests to the restcountry API in 
    /// order to validate the CountryOfOrigin field of the applicant class
    /// </summary>

    public class WebApiHelperClass
    {
        private HttpClient _httpClient;

        public WebApiHelperClass(HttpClient client)
        {
            _httpClient = client;
        }


        [HttpGet]
        public async Task<bool> validateCountry(string CountryOfOrigin)
        {

            var client = _httpClient;

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
