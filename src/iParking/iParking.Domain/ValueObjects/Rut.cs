using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace iParking.Domain.ValueObjects
{
    /// <summary>
    /// Representa un RUT chileno válido con su dígito verificador.
    /// Implementa validación del algoritmo módulo 11.
    /// </summary>
    public class Rut : IEquatable<Rut>
    {
        /// <summary>
        /// Número del RUT sin puntos ni guión (ej: "7654321")
        /// </summary>
        [Required]
        [StringLength(20, MinimumLength = 7)]
        public string Numero { get; }

        /// <summary>
        /// Dígito verificador (0-9 o K)
        /// </summary>
        [Required]
        [StringLength(1, MinimumLength = 1)]
        public string DigitoVerificador { get; }

        /// <summary>
        /// RUT formateado con puntos y guión (ej: "7.654.321-K")
        /// </summary>
        [JsonIgnore]
        public string Formatted => $"{FormatNumber(Numero)}-{DigitoVerificador.ToUpper()}";

        /// <summary>
        /// Crea una instancia de RUT validando el dígito verificador.
        /// </summary>
        public Rut(string numero, string digitoVerificador)
        {
            if (string.IsNullOrWhiteSpace(numero))
                throw new ArgumentException("El número del RUT no puede estar vacío", nameof(numero));

            if (string.IsNullOrWhiteSpace(digitoVerificador))
                throw new ArgumentException("El dígito verificador no puede estar vacío", nameof(digitoVerificador));

            Numero = new string(numero.Where(char.IsDigit).ToArray());
            DigitoVerificador = digitoVerificador.Trim().ToUpper();

            if (Numero.Length < 7 || Numero.Length > 20)
                throw new ArgumentException("El número del RUT debe tener entre 7 y 20 dígitos", nameof(numero));

            if (!IsValid(Numero, DigitoVerificador))
                throw new ArgumentException($"El RUT {Formatted} no es válido según el algoritmo módulo 11");
        }

        /// <summary>
        /// Valida un RUT usando el algoritmo módulo 11 chileno.
        /// </summary>
        public static bool IsValid(string numero, string dv)
        {
            if (string.IsNullOrWhiteSpace(numero) || string.IsNullOrWhiteSpace(dv))
                return false;

            var cleanNumber = new string(numero.Where(char.IsDigit).ToArray());
            if (cleanNumber.Length < 7)
                return false;

            var cleanDv = dv.Trim().ToUpper();
            if (cleanDv.Length != 1 || !(char.IsDigit(cleanDv[0]) || cleanDv[0] == 'K'))
                return false;

            int expectedDv = CalculateDV(cleanNumber);
            char expectedDvChar = expectedDv == 10 ? 'K' : expectedDv.ToString()[0];

            return cleanDv[0] == expectedDvChar;
        }

        private static int CalculateDV(string numero)
        {
            int sum = 0;
            int multiplier = 2;

            for (int i = numero.Length - 1; i >= 0; i--)
            {
                sum += (numero[i] - '0') * multiplier;
                multiplier = multiplier == 7 ? 2 : multiplier + 1;
            }

            int remainder = sum % 11;
            return remainder == 0 ? 0 : remainder == 1 ? 10 : 11 - remainder;
        }

        private static string FormatNumber(string numero)
        {
            var cleanNumber = new string(numero.Where(char.IsDigit).ToArray());
            return string.Join(".", 
                cleanNumber
                    .Reverse()
                    .Chunk(3)
                    .Select(chunk => new string(chunk.Reverse().ToArray()))
                    .Reverse());
        }

        public static Rut Parse(string rutCompleto)
        {
            if (string.IsNullOrWhiteSpace(rutCompleto))
                throw new ArgumentException("El RUT no puede estar vacío", nameof(rutCompleto));

            rutCompleto = rutCompleto.Trim().ToUpper();
            var parts = rutCompleto.Split('-', '.');
            
            string numero, dv;
            if (parts.Length >= 2)
            {
                numero = string.Join("", parts.Take(parts.Length - 1));
                dv = parts.Last().TrimStart('-').Trim();
            }
            else
            {
                var match = System.Text.RegularExpressions.Regex.Match(rutCompleto, @"^(\d+)(\d|K)$");
                if (!match.Success)
                    throw new ArgumentException($"Formato de RUT inválido: {rutCompleto}");

                numero = match.Groups[1].Value;
                dv = match.Groups[2].Value;
            }

            return new Rut(numero, dv);
        }

        public override string ToString() => Formatted;
        public override bool Equals(object? obj) => Equals(obj as Rut);
        public bool Equals(Rut? other) => other is not null && Numero == other.Numero && DigitoVerificador == other.DigitoVerificador;
        public override int GetHashCode() => HashCode.Combine(Numero, DigitoVerificador);
        public static bool operator ==(Rut? left, Rut? right) => Equals(left, right);
        public static bool operator !=(Rut? left, Rut? right) => !Equals(left, right);
    }
}
