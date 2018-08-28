using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ToggleAPI.Services;
using ToggleAPI.Models;
using ToggleAPI.Dtos;

namespace ToggleAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns>A list with all users</returns>
        /// <response code="200">Sucess</response>
        /// <response code="401">Unauthorized Access</response> 
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public IActionResult Get()
        {
            try
            {
                var userDto = _userService.Get();
                return Ok(userDto);    
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            
        }

        /// <summary>
        /// Get a specific user by id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/User/{id}
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>A specific user</returns>
        /// <response code="200">Returns the user informed by Id</response>
        /// <response code="400">If id is null or not found</response>
        /// <response code="401">Unauthorized Access</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Get(int id)
        {
            try
            {
                var userDto = _userService.Get(id);
                return Ok(userDto);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            
        }

        /// <summary>
        /// Create a New user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/User/
        ///     
        ///     {
        ///         "firstName": "string",
        ///         "lastName": "string",
        ///         "username": "string",
        ///         "password": "string",
        ///     }        
        /// </remarks>
        /// <returns>OK</returns>
        /// <response code="200">Sucess</response>
        /// <response code="400">If username exists or password is null</response>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Post([FromBody]UserDto userDto)
        {
            try
            {
                _userService.Post(userDto);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // User authentication passing name and password by json file
        /// <summary>
        /// Get the access token to use the API
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/User/auth
        ///     
        ///     {
        ///         "username": "string",
        ///         "password": "string",
        ///     }        
        /// </remarks>
        /// <returns>The user data and the security token</returns>
        /// <response code="200">Returns the user data and security token</response>
        /// <response code="400">If id is null or not found or the username is invalid</response>
        [AllowAnonymous]
        [HttpPost("auth")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Auth([FromBody]UserDto userDto)
        {
            try
            {
                return Ok(_userService.Auth(userDto.Username, userDto.Password));
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update the user informed by Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/User/auth
        ///     
        ///     {
        ///         "firstName": "string",
        ///         "lastName": "string",
        ///         "username": "string",
        ///         "password": "string",
        ///     }
        ///
        /// The fiedls username and password are optionals
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>OK</returns>
        /// <response code="200">Sucess</response>
        /// <response code="400">If the id is null or not found or the username is invalid</response>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]        
        public IActionResult Put(int id, [FromBody]UserDto userDto)
        {
            try
            {
                _userService.Put(id, userDto);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE api/User/{id}
        /// <summary>
        /// Delet a specific user by id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DEL api/User/{id}
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>OK</returns>
        /// <response code="200">Sucess</response>
        /// <response code="400">If id is null or not found</response>
        /// <response code="401">Unauthorized Access</response>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _userService.Delete(id);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}