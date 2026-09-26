using System.ComponentModel.DataAnnotations;
using iParking.Application.DTOs.User;

namespace iParking.Application.Validators
{
    /// <summary>
    /// Validador para registros de usuario.
    /// Aplica principio de Single Responsibility y validaciones centralizadas.
    /// </summary>
    public class UserValidator
    {
        public ValidationResult Validate(CreateUserDto user)
        {
            if (string.IsNullOrWhiteSpace(user.Rut))
                return ValidationResult.Failure("El RUT es obligatorio", 400);

            if (string.IsNullOrWhiteSpace(user.Dv))
                return ValidationResult.Failure("El dígito verificador es obligatorio", 400);

            if (string.IsNullOrWhiteSpace(user.Email))
                return ValidationResult.Failure("El email es obligatorio", 400);

            if (!IsValidEmail(user.Email))
                return ValidationResult.Failure("El email no tiene un formato válido", 400);

            if (string.IsNullOrWhiteSpace(user.Password))
                return ValidationResult.Failure("La contraseña es obligatoria", 400);

            if (user.Password.Length < 8)
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
