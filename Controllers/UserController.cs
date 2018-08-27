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

        // GET api/User
        [HttpGet]
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

        // GET api/User/{id}
        [HttpGet("{id}")]
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

        // POST api/User
        [AllowAnonymous]
        [HttpPost]
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

        // POST api/User/auth
        // User authentication passing name and password by json file
        [AllowAnonymous]
        [HttpPost("{auth}")]
        public IActionResult Authenticate([FromBody]UserDto userDto)
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
        // PUT api/User/{id}
        [HttpPut("{id}")]
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