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
            RuleFor(x => x.Name).NotEmpty().WithMessage(x => localizer["Name"] + " " + localizer["cant be null"]).MinimumLength(5).WithMessage(x => localizer["The name must have at least 5 characters"] + ".");
            RuleFor(x => x.FamilyName).NotEmpty().WithMessage(x => localizer["FamilyName"] + " " + localizer["cant be null"]).MinimumLength(5).WithMessage(x => localizer["The family name must contain at least 5 characters"] + ".");
            RuleFor(x => x.Address).NotEmpty().WithMessage(x => localizer["Address"] + " " + localizer["cant be null"]).MinimumLength(10).WithMessage(x => localizer["The adress must have be at least 10 characters long"] + ".");
            RuleFor(x => x.EMailAdress).NotEmpty().WithMessage(x => localizer["EMailAdress"] + " " + localizer["cant be null"]).EmailAddress().WithMessage(x => localizer["The Email Adress must be valid (it must contain an '@' character)"] + ".");
            RuleFor(x => x.Age).NotEmpty().WithMessage(x => localizer["Age"] + " " + localizer["cant be null"]).GreaterThanOrEqualTo(20).WithMessage(x => localizer["The age field must be between 20 and 60 years old"] + ".").LessThanOrEqualTo(60).WithMessage(x => localizer["The age field must be between 20 and 60 years old"] + ".");
            RuleFor(x => x.Hired).NotNull().WithMessage(x => localizer["Hired"] + " " + localizer["cant be null"]).WithMessage(x => localizer["The Hired field cant be null"]);
            RuleFor(x => x.CountryOfOrigin).NotEmpty().WithMessage(x => localizer["CountryOfOrigin"] + " " + localizer["cant be null"]).MustAsync(async (CountryOfOrigin, cancellation) =>
            {
                bool exist = await helper.validateCountry(CountryOfOrigin);
                return exist;
            }).WithMessage(x => localizer["The country must be a valid one (it must exist)"]);
        }
    }
}
