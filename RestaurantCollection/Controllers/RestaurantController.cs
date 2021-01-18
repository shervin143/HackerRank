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
    public class RestaurantController : Controller
    {
        private readonly ILogger<RestaurantController> _logger;
        private readonly IRepository _repository;

        public RestaurantController
        (
            ILogger<RestaurantController> logger,
            IRepository repository
        ) 
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(_repository.GetRestaurants());
        }

        [HttpGet("{city}")]
        public IActionResult Query(string city)
        {
            string message;

            if (string.IsNullOrWhiteSpace(city))
            {
                message = "City cannot be null or empty.";
                _logger.LogWarning(message);
                return StatusCode((int)HttpStatusCode.BadRequest, message);
            }

            var restaurantQueryModel = new RestaurantQueryModel
            {
                City = city
            };

            return Json(_repository.GetRestaurants(restaurantQueryModel));
        }

        [HttpGet("{id}")]
        public IActionResult Query(int id)
        {
            var restaurantQueryModel = new RestaurantQueryModel
            {
                Id = id
            };

            var restaurants = _repository.GetRestaurants(restaurantQueryModel).Result;
            if (restaurants == null || !restaurants.Any())
            {
                string message = $"There is no resturant for the given id.";
                _logger.LogError(message);
                return StatusCode((int)HttpStatusCode.BadRequest, message);
            }

            return Json(restaurants.FirstOrDefault());
        }

        [HttpGet]
        public IActionResult Sort(int id)
        {
            return Json(_repository.GetRestaurantsSorted());
        }

        [HttpPost]
        public IActionResult Post([FromBody] Restaurant restaurant)
        {
            string message;

            if (restaurant == null)
            {
                message = "Restaurant is null. Unable to add resturant.";
                _logger.LogWarning(message);
                return StatusCode((int)HttpStatusCode.BadRequest, message);
            }
            
            try
            {
                _repository.AddRestaurant(restaurant);
            }
            catch(Exception ex)
            {
                message = $"An unexpected error occurred while updating Restaurant '{restaurant.Name}'. {ex.Message}";
                _logger.LogError(message);
                return StatusCode((int)HttpStatusCode.BadRequest, message);
            }

            return Json(Ok());            
        }

        [HttpPut]
        public IActionResult Put(int id, [FromBody] Restaurant restaurant)
        {
            string message;

            if(restaurant == null)
            {
                message = "Restaurant is null. Unable to update resturant.";
                _logger.LogWarning(message);
                return StatusCode((int)HttpStatusCode.BadRequest, message);
            }

            var restaurantQueryModel = new RestaurantQueryModel
            {
                Id = id
            };

            var restaurants = _repository.GetRestaurants(restaurantQueryModel).Result;
            if (restaurants == null || !restaurants.Any())
            {
                message = $"There is no resturant for the given id.";
                _logger.LogError(message);
                return StatusCode((int)HttpStatusCode.BadRequest, message);
            }

            var resturant = restaurants.FirstOrDefault();
            resturant.AverageRating = restaurant.AverageRating;
            resturant.Votes = restaurant.Votes;

            _repository.UpdateRestaurant(resturant);
            return Json(Ok());
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var restaurantQueryModel = new RestaurantQueryModel
            {
                Id = id
            };

            var restaurants = _repository.GetRestaurants(restaurantQueryModel).Result;
            if(restaurants == null || !restaurants.Any())
            {
                string message = $"There is no resturant for the given id.";
                _logger.LogError(message);
                return StatusCode((int)HttpStatusCode.BadRequest, message);
            }

            _repository.DeleteRestaurant(restaurants.FirstOrDefault());
            return Json(Ok());
        }
    }
}
