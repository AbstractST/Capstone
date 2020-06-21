﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClicknEat.Contracts.V1;
using ClicknEat.Contracts.V1.Requests;
using ClicknEat.Contracts.V1.Responses;
using ClicknEat.Data;
using ClicknEat.Domain;
using ClicknEat.Extensions;
using ClicknEat.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClicknEat.Controllers.V1
{
    [ApiController]
    [Route(ApiRoutes.Restaurants.Route)]
    public class RestaurantsController : Controller
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        /*private readonly IUriService _uriService;*/

        public RestaurantsController(IRestaurantService restaurantService, IMapper mapper, ApplicationDbContext context)
        {
            _restaurantService = restaurantService;
            _mapper = mapper;
            _context = context;
            /*_uriService = uriService;*/
        }

        /// <summary>
        /// Returns all Restaurants in the system
        /// </summary>
        /// <response code="200">Returns all Restaurants in the system</response>
        [HttpGet(ApiRoutes.Restaurants.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] string restaurantName)
        {
            var restaurants = await _restaurantService
                .GetAllAsync(restaurantName);

            return Ok(_mapper.Map<List<Restaurant>, List<RestaurantResponse>>(restaurants));
        }

        [HttpGet(ApiRoutes.Restaurants.Get)]
        public async Task<IActionResult> GetRestaurant([FromRoute] Guid restaurantId)
        {
            var restaurant = await _restaurantService.GetRestaurantAsync(restaurantId);

           /* foreach (var item in restaurant)
            {*/
                if (restaurant.Id != null && restaurant.Id != Guid.Empty && restaurant.Id == restaurantId)
            return Ok(new Response<RestaurantProductResponse>(_mapper.Map<RestaurantProductResponse>(restaurant)));

            
            return NotFound();
                    /*return Ok(_mapper.Map<List<Restaurant>, List<RestaurantProductResponse>>(restaurant));*/
        }

        /// <summary>
        /// Creates Restaurant in the system
        /// </summary>
        /// <remarks>
        /// Sample **request**:
        /// 
        ///     POST /api/v1/CreateRestaurant
        ///     {
        ///         restaurantName": "Some Name",
        ///         "description": "Description ",
        ///         "categoryToRestaurantRequest": {
        ///         "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "restaurantCategoryName": "string"
        ///     }
        /// </remarks>
        /// <response code="200">Returns all Restaurants in the system</response>
        /// <response code="400">Unable to create Restaurant</response>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles ="Admin")]
        [HttpPost(ApiRoutes.Restaurants.Create)]
        public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantRequest createRestaurantRequest)
        {
            var newRestaurantId = new Guid();

            var query = await _context.RestaurantCategories
                .Where(x => x.Id == createRestaurantRequest
                .CategoryToRestaurantRequest.Id)
                .FirstOrDefaultAsync();

            if (query == null)
                return BadRequest();

            var restaurant = new Restaurant
            {
                Id = newRestaurantId,
                RestaurantName = createRestaurantRequest.RestaurantName,
                Description = createRestaurantRequest.Description,
                RestaurantCategory = query
            };

            if (restaurant == null)
                return BadRequest();

            await _restaurantService
                .CreateRestaurantAsync(restaurant);

            return Ok(new Response<RestaurantResponse>(_mapper.Map<RestaurantResponse>(restaurant)));

            /*var locationUri = _uriService.GetRestaurantUri(restaurant.Id.ToString());
            return Created(locationUri, new Response<RestaurantResponse>(_mapper.Map<RestaurantResponse>(restaurant)));*/
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut(ApiRoutes.Restaurants.Update)]
        public async Task<IActionResult> UpdateRestaurant([FromRoute] Guid restaurantId, [FromBody] UpdateRestaurantRequest updateRestaurantRequest)
        {
      /*      var userOwnsRestaurant = await _restaurantService
                .UserOwnsRestaurantAsync(restaurantId, HttpContext.GetUserId());

            if (!userOwnsRestaurant)
            {
                return BadRequest();
            }*/

            var restaurant = await _restaurantService
                .GetRestaurantByIdAsync(restaurantId);

            var query = await _context.RestaurantCategories
                .Where(x => x.Id == updateRestaurantRequest
                .CategoryToRestaurantRequest.Id)
                .FirstOrDefaultAsync();

            if (query == null || restaurant == null)
                return BadRequest();

            restaurant.RestaurantName = updateRestaurantRequest.RestaurantName;
            restaurant.Description = updateRestaurantRequest.Description;
            restaurant.RestaurantCategory = query;

            var updated = await _restaurantService
                .UpdateRestaurantAsync(restaurant);

            if (updated)
                return Ok(new Response<RestaurantResponse>(_mapper.Map<RestaurantResponse>(restaurant)));

                return BadRequest();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete(ApiRoutes.Restaurants.Delete)]
        public async Task<IActionResult> DeteleRestaurant([FromRoute] Guid restaurantId)
        {
          /*  var userOwnsRestaurant = await _restaurantService
            .UserOwnsRestaurantAsync(restaurantId, HttpContext.GetUserId());

            if (!userOwnsRestaurant)
            {
                return BadRequest();
            }*/

            var deleted = await _restaurantService
                .DeleteRestaurantAsync(restaurantId);

            if (deleted)
                return NoContent();

            return BadRequest();
        }
    }
}