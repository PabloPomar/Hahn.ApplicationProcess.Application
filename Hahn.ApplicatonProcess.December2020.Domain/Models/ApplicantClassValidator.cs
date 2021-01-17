using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.December2020.Domain.Models
{
    public class ApplicantClassValidator : AbstractValidator<ApplicantClass>
    {

        private readonly IHttpClientFactory _clientFactory;

        public ApplicantClassValidator(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            var stringopts = new StringSplitOptions(); 
            RuleFor(x => x.Name).NotEmpty().MinimumLength(5);
            RuleFor(x => x.FamilyName).NotEmpty().MinimumLength(5);
            RuleFor(x => x.Address).NotEmpty().MinimumLength(10);
            RuleFor(x => x.EMailAdress).NotEmpty().EmailAddress();
            //RuleFor(x => x.EMailAdress.Split('@', stringopts)[1]).NotEmpty().MinimumLength(1).Must(x => x.Contains('.') && x.First() != ('.'));
            RuleFor(x => x.EMailAdress.Split('@', stringopts)[1]).NotEmpty().MinimumLength(1);
            RuleFor(x => x.Age).NotEmpty().GreaterThanOrEqualTo(20).LessThanOrEqualTo(60);
            RuleFor(x => x.Hired).NotNull();
            RuleFor(x => x.CountryOfOrigin).NotEmpty();
            RuleFor(x => validateCountry(x.CountryOfOrigin).Result).NotEmpty().Equal(true);
        }       

        public async Task<bool> validateCountry(string CountryOfOrigin)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://restcountries.eu/rest/v2/name/" + CountryOfOrigin);
            request.Headers.Add("fullText", "true");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
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
