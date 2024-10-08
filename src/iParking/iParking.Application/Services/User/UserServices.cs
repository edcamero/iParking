﻿using iParking.DataAccess.DataServices;
using iParking.Domain.Entities;
using iParking.Domain.Entities.Usuario;

namespace iParking.Application.Services.User
{
    public  class UserServices: IUserServices
    {
        private readonly IUserDataServices _userDataServices;

        public UserServices(IUserDataServices userDataServices)
        {
            _userDataServices = userDataServices ?? throw new ArgumentNullException(nameof(userDataServices));
        }

        public async Task<ActionResponseSession> CreatedUser(UsuarioNuevo nuevoUsuario)
        {
            var user = await _userDataServices.GetUserAsync(nuevoUsuario.Rut, nuevoUsuario.DigVer);

            var response = new ActionResponseSession();

            if(user != null && user.Estado == 0)
            {
                response.Message = "El usuario ya se encuentra registrado pero esta deshabilitado";
                response.Code = 409;

                return response;
            }

            if(user != null && user.Dv.Equals(nuevoUsuario.DigVer) && user!.Rut.Equals(nuevoUsuario.Rut) && user.ClaveAcceso.Equals(nuevoUsuario.ClaveAcceso))
            {
                response.Status = true;
                response.Code = 201;
                response.KeySession = user.IdUsuario;
                response.Id = user.IdUsuario;

                return response;
            }

           var userId = await _userDataServices.CreatedUser(nuevoUsuario);            

            if(userId > 0)
            {
                response.Status = true;
                response.Code = 201;
                response.KeySession = userId;
                response.Id = userId;

            }
            else
            {
                response.Message = "Error tratando de crear el usuario";
            }

            return response;
        }
    }
}
