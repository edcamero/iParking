namespace iParking.Domain.Enums
{
    /// <summary>
    /// Roles disponibles en el sistema según los requerimientos.
    /// </summary>
    public enum UserRole
    {
        SuperAdmin = 1,       // Gestiona la plataforma SaaS
        CompanyAdmin = 2,     // Admin de empresa (crea parqueaderos, ve reportes)
        Operator = 3,         // Operador de sitio (entradas/salidas)
        Client = 4            // Conductor (app/web)
    }

    /// <summary>
    /// Categorías de vehículos para diferenciación de tarifas y capacidad.
    /// </summary>
    public enum VehicleType
    {
        Car = 1,
        Motorcycle = 2,
        Truck = 3,
        Bicycle = 4
    }

    /// <summary>
    /// Estados posibles de un puesto de estacionamiento.
    /// </summary>
    public enum ParkingSpotStatus
    {
        Available = 1,
        Occupied = 2,
        Reserved = 3,
        Maintenance = 4
    }

    /// <summary>
    /// Tipos de tarifa aplicables.
    /// </summary>
    public enum RateType
    {
        Fractional = 1,   // Por minuto/hora
        Daily = 2,        // Día completo
        Overnight = 3,    // Pernocta
        Subscription = 4, // Mensualidad
        TimeFrame = 5     // Franja horaria fija (ej: Plan Nocturno, Plan Oficina)
    }

    /// <summary>
    /// Medios de pago soportados.
    /// Nota: Las tarjetas de crédito/débito se procesan mediante pasarelas externas (Stripe, PayPal, etc.).
    /// El sistema NO almacena datos sensibles de tarjetas PCI-DSS.
    /// </summary>
    public enum PaymentMethod
    {
        Cash = 1,         // Efectivo
        DebitCard = 2,    // Tarjeta débito (procesada externamente)
        CreditCard = 3,   // Tarjeta crédito (procesada externamente)
        Transfer = 4,     // Transferencia/QR
        DigitalWallet = 5,// Billeteras digitales
        Recurrent = 6     // Cobro recurrente en línea (vía token de pasarela)
    }

    /// <summary>
    /// Estado de una sesión de estacionamiento.
    /// </summary>
    public enum ParkingSessionStatus
    {
        Active = 1,
        Completed = 2,
        Cancelled = 3
    }

    /// <summary>
    /// Tipo de excedente de franja horaria.
    /// </summary>
    public enum ExcessType
    {
        EarlyEntry = 1,   // Ingreso anticipado
        LateExit = 2      // Salida tardía
    }

    /// <summary>
    /// Estado del cobro de excedente.
    /// </summary>
    public enum ExcessPaymentStatus
    {
        Pending = 1,      // Pendiente de cobro
        Paid = 2,         // Pagado exitosamente
        Failed = 3,       // Cobro fallido
        Deferred = 4      // Diferido a facturación posterior
    }
}
