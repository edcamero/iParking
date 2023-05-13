create model and contexto 

```

dotnet ef dbcontext scaffold "Data Source=localhost\SQLEXPRESS;Initial Catalog=parking; Integrated Security=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Models --context ParkingContext

```