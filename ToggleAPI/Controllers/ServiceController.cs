using ToggleAPI.Services;
using ToggleAPI.Models;
using ToggleAPI.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ToggleAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private IServiceService _serviceService;
        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        /// <summary>
        /// Get all services.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/Service/
        ///
        /// </remarks>
        /// <returns>A list with all services</returns>
        /// <response code="200">Returns a list with all services</response>
        /// <response code="401">Unauthorized Access</response> 
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public IActionResult Get()
        {
            try
            {
                var serviceDto = _serviceService.Get();
                return Ok(serviceDto);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            
        }

        /// <summary>
        /// Get a specific Service by id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/Service/{id}
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>A specific service</returns>
        /// <response code="200">Returns the service informed by Id</response>
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
                var serviceDto = _serviceService.Get(id);
                return Ok(serviceDto);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            
        }

        /// <summary>
        /// Create a New Service
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/Service/
        ///     
        ///     {
        ///         "version": int,
        ///         "Name": "string"
        ///     }        
        /// </remarks>
        /// <returns>OK</returns>
        /// <response code="200">Sucess</response>
        /// <response code="400">If version or name is null</response>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Post([FromBody]ServiceDto serviceDto)
        {
            try
            {
                _serviceService.Post(serviceDto);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update the service informed by Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/Service/
        ///     
        ///     {
        ///         "version": int,
        ///         "name": "string"
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="serviceDto"></param>
        /// <returns>OK</returns>
        /// <response code="200">Sucess</response>
        /// <response code="400">If the version or name is null</response>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]        
        public IActionResult Put(int id, [FromBody]ServiceDto serviceDto)
        {
            try
            {
                _serviceService.Put(id, serviceDto);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delet a specific service by id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE api/Service/{id}
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
                _serviceService.Delete(id);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get all Toggles from a specific service by id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/Service/toggles/{id}
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>A list with all Toggles and the most important one of they</returns>
        /// <response code="200">Return a list with all Toggles and the most important one of they</response>
        /// <response code="400">If id is null or not found</response>
        /// <response code="401">Unauthorized Access</response>
        [HttpGet("toggles/{id}")]
        public IActionResult GetToggles(int id)
        {
            try
            {
                var serviceToggles = _serviceService.GetAllToggles(id);
                return Ok(serviceToggles);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
    }
}