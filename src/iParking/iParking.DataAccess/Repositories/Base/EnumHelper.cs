using System;
using System.Data;

namespace iParking.DataAccess.Repositories.Base
{
    /// <summary>
    /// Helper para conversión segura de enums en operaciones de base de datos.
    /// Centraliza la lógica de mapeo entre tipos SQL (int/string) y enums C#.
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Convierte un valor de base de datos a enum de forma segura.
        /// Retorna el valor por defecto del enum si la conversión falla.
        /// </summary>
        public static TEnum MapEnum<TEnum>(object? value) where TEnum : struct, Enum
        {
            if (value == null || value == DBNull.Value)
                return default;

            if (value is int intValue)
                return Enum.IsDefined(typeof(TEnum), intValue)
                    ? (TEnum)(object)intValue
                    : default;

            if (value is string stringValue)
                return Enum.TryParse<TEnum>(stringValue, true, out var result)
                    ? result
                    : default;

            return default;
        }

        /// <summary>
        /// Convierte un valor de base de datos a enum nullable.
        /// Retorna null si el valor es NULL en la BD o la conversión falla.
        /// </summary>
        public static TEnum? MapEnumNullable<TEnum>(object? value) where TEnum : struct, Enum
        {
            if (value == null || value == DBNull.Value)
                return null;

            if (value is int intValue && Enum.IsDefined(typeof(TEnum), intValue))
                return (TEnum)(object)intValue;

            if (value is string stringValue && Enum.TryParse<TEnum>(stringValue, true, out var result))
                return result;

            return null;
        }

        /// <summary>
        /// Convierte un enum a su valor entero para parámetro SQL.
        /// </summary>
        public static int ToInt<TEnum>(TEnum value) where TEnum : struct, Enum
        {
            return Convert.ToInt32(value);
        }

        /// <summary>
        /// Convierte un enum nullable a su valor entero para parámetro SQL.
        /// Retorna DBNull.Value si el enum es null.
        /// </summary>
        public static object ToIntNullable<TEnum>(TEnum? value) where TEnum : struct, Enum
        {
            return value.HasValue ? (object)Convert.ToInt32(value.Value) : DBNull.Value;
        }
    }
}
