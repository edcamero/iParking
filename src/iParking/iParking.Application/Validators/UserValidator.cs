using System.ComponentModel.DataAnnotations;
using iParking.Domain.Entities.Usuario;

namespace iParking.Application.Validators
{
    /// <summary>
    /// Validador para registros de usuario.
    /// Aplica principio de Single Responsibility y validaciones centralizadas.
    /// </summary>
    public class UserValidator
    {
        public ValidationResult Validate(UsuarioNuevo usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Rut))
                return ValidationResult.Failure("El RUT es obligatorio", 400);

            if (string.IsNullOrWhiteSpace(usuario.DigVer))
                return ValidationResult.Failure("El dígito verificador es obligatorio", 400);

            if (string.IsNullOrWhiteSpace(usuario.Mail))
                return ValidationResult.Failure("El email es obligatorio", 400);

            if (!IsValidEmail(usuario.Mail))
                return ValidationResult.Failure("El email no tiene un formato válido", 400);

            if (string.IsNullOrWhiteSpace(usuario.ClaveAcceso))
                return ValidationResult.Failure("La contraseña es obligatoria", 400);

            if (usuario.ClaveAcceso.Length < 8)
                return ValidationResult.Failure("La contraseña debe tener al menos 8 caracteres", 400);

            return ValidationResult.Success();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Resultado de validación.
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; private set; }
        public string ErrorMessage { get; private set; } = string.Empty;
        public int Code { get; private set; }

        private ValidationResult(bool isValid, string errorMessage, int code)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
            Code = code;
        }

        public static ValidationResult Success() => new ValidationResult(true, string.Empty, 200);
        public static ValidationResult Failure(string error, int code = 400) => new ValidationResult(false, error, code);
    }
}
