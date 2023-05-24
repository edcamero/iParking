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
        public ParkingController(IParkingServices parkingServices)
        {
            _parkingServices = parkingServices ?? throw new ArgumentNullException(nameof(parkingServices));
        }

        // GET: api/<ParkingController>
        [HttpGet]
        public async Task<IEnumerable<Parking>> GetAsync()
        {
            return await _parkingServices.GetParkings();
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
