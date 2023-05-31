using iParking.Application.Services.Parking;
using iParking.DataAccess.Models;
using iParking.Domain.Parking;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace iParking.API.Controllers
{
    [Route("api/v1/parking")]
    [ApiController]
    public class ParkingController : ControllerBase
    {
        private readonly IParkingServices _parkingServices;
        private readonly ILogger<ParkingController> _logger;
        public ParkingController(IParkingServices parkingServices, ILogger<ParkingController> logger)
        {
            _parkingServices = parkingServices ?? throw new ArgumentNullException(nameof(parkingServices));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/<ParkingController>
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var result =  await _parkingServices.GetParkings();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error al consultar los parquederos");
                return StatusCode(500, "Error de servidor");
            }
           
        }

        // GET api/<ParkingController>/5
        [HttpGet("{id}")]
        public async Task<DataAccess.Models.Parking> Get(int id)
        {
            return await _parkingServices.GetParking(id);
        }

        // POST api/<ParkingController>
        [HttpPost]
        public async Task<DataAccess.Models.Parking> Post([FromBody] ParkingInputDTO parkingInput)
        {
            return await _parkingServices.CreateParking(parkingInput);
        }

        // PUT api/<ParkingController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ParkingInputUpdateDTO parkingInput)
        {
            var parking = await _parkingServices.UpdateParking(parkingInput, id);

            if (parking != null)
            {
                return Ok(parking);
            }

            return NotFound();
        }

        // DELETE api/<ParkingController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var parking = await _parkingServices.DeleteParking(id);

            if (parking != null)
            {
                return Ok(parking);
            }

            return NotFound();
        }
    }
}
