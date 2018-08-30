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
    public class ToggleController : ControllerBase
    {
        private IToggleService _toggleService;
        public ToggleController(IToggleService toggleService)
        {
            _toggleService = toggleService;
        }

        /// <summary>
        /// Get all toggles.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/Toggle/
        ///
        /// </remarks>
        /// <returns>A list with all toggles</returns>
        /// <response code="200">Returns a list with all toggles</response>
        /// <response code="401">Unauthorized Access</response> 
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public IActionResult Get()
        {
            try
            {
                var toggleDto = _toggleService.Get();
                return Ok(toggleDto);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            
        }

        /// <summary>
        /// Get a specific Toggle by id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/Toggle/{id}
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>A specific toggle</returns>
        /// <response code="200">Returns the toggle informed by Id</response>
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
                var toggleDto = _toggleService.Get(id);
                return Ok(toggleDto);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            
        }

        /// <summary>
        /// Create a New Toggle
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/Toggle/
        ///     
        ///     To include All Services
        ///     {
        ///     	"Name": "string",
        ///     	"State": bolean,
        ///     	"Value": int,
        ///     	"ServicesList": [ "All" ]
        ///     }
        ///
        ///     To include The Services 1,2,3 (By Id)
        ///     {
        ///     	"Name": "string",
        ///     	"State": bolean,
        ///     	"Value": int,
        ///     	"ServicesList": [ 1,2,3 ]
        ///     }        
        ///
        /// </remarks>
        /// <returns>OK</returns>
        /// <response code="200">Sucess</response>
        /// <response code="400">If the fields name or ServicesList are null.</response>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Post([FromBody]SavingToggleDto toggleDto)
        {
            try
            {
                _toggleService.Post(toggleDto);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update the toggle informed by Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/Toggle/
        ///     
        ///     {
        ///         "version": int,
        ///         "name": "string"
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="toggleDto"></param>
        /// <returns>OK</returns>
        /// <response code="200">Sucess</response>
        /// <response code="400">If the version or name is null</response>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]        
        public IActionResult Put(int id, [FromBody]ToggleDto toggleDto)
        {
            try
            {
                _toggleService.Put(id, toggleDto);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delet a specific toggle by id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE api/Toggle/{id}
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>OK</returns>
        /// <response code="200">Sucess</response>
        /// <response code="400">If id is null or not found</response>
        /// <response code="401">Unauthorized Access</response>
        [HttpDelete("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Delete(int id)
        {
            try
            {
                _toggleService.Delete(id);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }        
    }
}