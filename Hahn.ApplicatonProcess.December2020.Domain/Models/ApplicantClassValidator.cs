using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.December2020.Domain.Models
{

    /// <summary>
    /// class manage the validation of the ApplicantClass to make sure the form is valid before submiting it. 
    /// </summary>
    public class ApplicantClassValidator : AbstractValidator<ApplicantClass>
    {

        HttpClient _client;     

        public ApplicantClassValidator(HttpClient client, IStringLocalizer<ApplicantClass> localizer)
        {
            //
            //ApplicantClass helper = new ApplicantClass();
            _client = client;
            WebApiHelperClass helper = new WebApiHelperClass(_client);
            RuleFor(x => x.ID).NotNull().WithMessage(x => localizer["The ID cant be null"]);
            RuleFor(x => x.Name).NotEmpty().MinimumLength(5).WithMessage(x => localizer["The name must have at least 5 characters"]);
            RuleFor(x => x.FamilyName).NotEmpty().MinimumLength(5).WithMessage(x => localizer["The family name must contain at least 5 characters"]);
            RuleFor(x => x.Address).NotEmpty().MinimumLength(10).WithMessage(x => localizer["The adress must have be at least 10 characters long"]);
            RuleFor(x => x.EMailAdress).NotEmpty().EmailAddress().WithMessage(x => localizer["The Email Adress must be valid (it must contain an '@' character)"]);
            RuleFor(x => x.Age).NotEmpty().GreaterThanOrEqualTo(20).LessThanOrEqualTo(60).WithMessage(x => localizer["The age field must be between 20 and 60 years old"]);
            RuleFor(x => x.Hired).NotNull().WithMessage(x => localizer["The Hired field cant be null"]);
            RuleFor(x => x.CountryOfOrigin).NotEmpty().MustAsync(async (CountryOfOrigin, cancellation) =>
            {
                bool exist = await helper.validateCountry(CountryOfOrigin);
                return exist;
            }).WithMessage(x => localizer["The country must be a valid one (it must exist)"]);
        }
    }
}
