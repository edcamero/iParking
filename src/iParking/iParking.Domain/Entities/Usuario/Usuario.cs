using System;
using System.ComponentModel.DataAnnotations;
using iParking.Domain.Enums;

namespace iParking.Domain.Entities.Usuario
{
    /// <summary>
    /// Entidad de usuario de dominio compatible con TBL_USUARIOS y el ecosistema iParking.
    /// Contiene datos del perfil, credenciales, información de dispositivo y pertenencia a empresa (Multi-Tenant).
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Identificador único del usuario (clave primaria en TBL_USUARIOS).
        /// </summary>
        [Key]
        public int IdUsuario { get; set; }

        /// <summary>
        /// Número de RUT chileno (sin puntos ni guion).
        /// </summary>
        public string Rut { get; set; } = string.Empty;

        /// <summary>
        /// Dígito verificador del RUT (0-9 o K).
        /// </summary>
        public string Dv { get; set; } = string.Empty;

        /// <summary>
        /// Nombres del usuario.
        /// </summary>
        public string Nombres { get; set; } = string.Empty;

        /// <summary>
        /// Apellidos del usuario.
        /// </summary>
        public string Apellidos { get; set; } = string.Empty;

        /// <summary>
        /// Correo electrónico del usuario (usado para notificaciones y acceso).
        /// </summary>
        public string Mail { get; set; } = string.Empty;

        /// <summary>
        /// Número telefónico de contacto.
        /// </summary>
        public string Telefono { get; set; } = string.Empty;

        /// <summary>
        /// Estado numérico del usuario en la base de datos (1 = Activo, 0 = Inactivo).
        /// </summary>
        public int Estado { get; set; } = 1;

        /// <summary>
        /// Hash o clave de acceso del usuario para autenticación.
        /// </summary>
        public string ClaveAcceso { get; set; } = string.Empty;
        
        /// <summary>
        /// Identificador de la empresa a la que pertenece el usuario (Modelo SaaS Multi-Tenant).
        /// </summary>
        public int? CompanyId { get; set; }

        /// <summary>
        /// Rol asignado al usuario dentro de la plataforma.
        /// </summary>
        public UserRole Role { get; set; } = UserRole.Operator;

        /// <summary>
        /// Identificador IMEI del dispositivo móvil del usuario / POS.
        /// </summary>
        public string? ImeiCelular { get; set; }

        /// <summary>
        /// Número de serie del dispositivo celular.
        /// </summary>
        public string? SerieCelular { get; set; }

        /// <summary>
        /// Versión de la aplicación móvil instalada.
        /// </summary>
        public string? VersionApp { get; set; }

        /// <summary>
        /// Ciudad de residencia u operación.
        /// </summary>
        public string? Ciudad { get; set; }

        /// <summary>
        /// Fecha y hora en formato UTC del último inicio de sesión.
        /// </summary>
        public DateTime? LastLoginDate { get; set; }

        /// <summary>
        /// Obtiene el Value Object de RUT validado si el número y DV están disponibles.
        /// </summary>
        public ValueObjects.Rut? GetRutValueObject()
        {
            return ValueObjects.Rut.TryParse(Rut, Dv, out var rutVo) ? rutVo : null;
        }

        /// <summary>
        /// Obtiene o establece el estado tipado del usuario como enum EstadoUsuario.
        /// </summary>
        public EstadoUsuario EstadoTipado
        {
            get => Enum.IsDefined(typeof(EstadoUsuario), Estado) ? (EstadoUsuario)Estado : EstadoUsuario.Inactivo;
            set => Estado = (int)value;
        }
    }
}
