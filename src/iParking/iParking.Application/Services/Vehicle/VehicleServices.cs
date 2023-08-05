using iParking.DataAccess.DataServices.VehicleDataServices;
using iParking.Domain.Entities;
using iParking.Domain.Entities.Vehicle;

namespace iParking.Application.Services.Vehicle
{
    public class VehicleServices : IVehicleServices
    {
        private readonly IVehicleDataServices _vehicleDataServices;

        public VehicleServices(IVehicleDataServices vehicleDataServices)
        {
            _vehicleDataServices = vehicleDataServices;
        }

        public async Task<(List<EVehicle>, ActionResponse)> GetUserVehicles(int userId)
        {

            var vehicles = await _vehicleDataServices.GetUserVehicles(userId);

            var response = new ActionResponse();

            if (vehicles.Count == 0)
            {
                response.Message = "El usuario no tiene vehiculos registrados";
                response.Code = 404;
            }
            else
            {
                response.Message = string.Format("El usuario tiene {0} vehiculos registrados", vehicles.Count);
                response.Code = 200;
            }

            return (vehicles, response);
        }


        public async Task<ActionResponseSession> CreatedUserVehicle(VehicleUserInput vehicleInput)
        {
            var vehiclesUser = await _vehicleDataServices.GetUserVehicles(int.Parse(vehicleInput.KeySesion));

            var vehicleUser = vehiclesUser.Count != 0 ? vehiclesUser.First(x => x.Placa == vehicleInput.Placa) : null;

            var response = new ActionResponseSession();

            if (vehicleUser != null)
            {
                response.Message = vehicleUser.Estado == 0 ? "El usuario ya se tiene vehiculo registrado pero esta deshabilitado" : "El vehiculo ya se encuentra registrado";
                response.Code = 409;

                return response;
            }

            var vehicle = await _vehicleDataServices.CreatedUserVehicle(vehicleInput);

            if (vehicle > 0)
            {
                response.Status = true;
                response.Code = 201;
                response.KeySession = vehicle;
                response.Id = vehicle;

            }
            else
            {
                response.Message = "Error tratando de registrar el vehiculo";
            }

            return response;
        }

        public async Task<ActionResponseSession> UpdatedUserVehicleDefault(int userId, int vehicleId)
        {
            var vehiclesUser = await _vehicleDataServices.GetUserVehicles(userId);

            var vehicleUser = vehiclesUser.Count != 0 ? vehiclesUser.First(x => x.IdPlaca == vehicleId) : null;

            var response = new ActionResponseSession();

            if (vehicleUser == null)
            {
                response.Message = "El vehiculo no se encuentra registrado";
                response.Code = 409;

                return response;
            }


            if (vehicleUser.Estado == 0)
            {
                if (vehicleUser != null)
                {
                    response.Message = "El usuario tiene vehiculo registrado pero esta deshabilitado";
                    response.Code = 409;

                    return response;
                }
            }

            var isVehicleUpdated = await _vehicleDataServices.UpdatedUserVehicleDefault(userId, vehicleId);

            if (isVehicleUpdated)
            {
                response.Status = true;
                response.Code = 201;
                response.KeySession = userId;
                response.Id = vehicleId;
            }
            else
            {
                response.Message = "Error tratando de registrar el vehiculo";
            }

            return response;
        }

        public async Task<ActionResponseSession> DeletedUserVehicle(int userId, int vehicleId)
        {
            var vehiclesUser = await _vehicleDataServices.GetUserVehicles(userId);

            var vehicleUser = vehiclesUser.Count != 0 ? vehiclesUser.First(x => x.IdPlaca == vehicleId) : null;

            var response = new ActionResponseSession();

            if (vehicleUser == null)
            {
                response.Message = "El vehiculo no se encuentra registrado";
                response.Code = 409;

                return response;
            }


            if (vehicleUser.Estado == 0)
            {
                if (vehicleUser != null)
                {
                    response.Message = "El usuario tiene vehiculo registrado pero esta deshabilitado";
                    response.Code = 409;

                    return response;
                }
            }

            var isVehicleUpdated = await _vehicleDataServices.DeleteUserVehicle(userId, vehicleId);

            if (isVehicleUpdated)
            {
                response.Status = true;
                response.Code = 201;
                response.KeySession = userId;
                response.Id = vehicleId;
            }
            else
            {
                response.Message = "Error tratando de eliminar el vehiculo";
            }

            return response;
        }
    }
}
