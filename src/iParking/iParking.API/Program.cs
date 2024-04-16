using iParking.Application;
using iParking.DataAccess;
using iParking.Infrastructure.Services;
using Serilog;

const string IParkingConnection = "iParkingConnection";

// Add services to the logger.
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
Log.Logger = new LoggerConfiguration()
          .ReadFrom.Configuration(builder.Configuration)
          .CreateLogger();

// Add services to the container.
builder.Services.AddHttpClient();

builder.Services.AddiParkingDataServices(builder.Configuration.GetConnectionString(IParkingConnection) ?? throw new ArgumentNullException(nameof(IParkingConnection)));
        
builder.Services.AddScoped<IIntegrationServiceClient, IntegrationServiceClient>();

builder.Services.AddiParkingAplicationServices();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddControllers().AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
//});


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
