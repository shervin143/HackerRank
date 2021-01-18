using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RestaurantCollection.WebApi.DataAccess;
//using RestaurantCollection.WebApi.DTO.Common;
using RestaurantCollection.WebApi.DTO.Forms;
//using RestaurantCollection.WebApi.DTO.ViewModels;
using RestaurantCollection.WebApi.Models;
using System.Net;

namespace RestaurantCollection.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class CompaniesController : Controller
    {
        private readonly ILogger<CompaniesController> _logger;
        private readonly IRepository _repository;

        public CompaniesController
        (
            ILogger<CompaniesController> logger,
            IRepository repository
        )
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Company company)
        {
            string message;

            if (company == null)
            {
                message = "Company is null. Unable to add company.";
                _logger.LogWarning(message);
                return StatusCode((int)HttpStatusCode.BadRequest, message);
            }

            if(string.IsNullOrEmpty(company.companyName) || company.companyName.Length < 5 || company.companyName.Length > 35 || !company.companyName.StartsWith("Company Name:"))
            {
                message = "CompanyName is invalid: CompanyName must contain a minimum of 5 characters and a maximum if 35, and it must start with 'Company Name:'";
                _logger.LogWarning(message);
                return StatusCode((int)HttpStatusCode.BadRequest, message);
            }

            if(company.NumberOfEmployees < 1)
            {
                message = "NumberOfEmployees is invalid: NumberOfEmployees must be greater than 1";
                _logger.LogWarning(message);
                return StatusCode((int)HttpStatusCode.BadRequest, message);
            }

            if (company.AverageSalary < 0)
            {
                message = "AverageSalary is invalid: AverageSalary must be greater than 0";
                _logger.LogWarning(message);
                return StatusCode((int)HttpStatusCode.BadRequest, message);
            }

            return Json(Ok());
        }
    }
}
