using DiscoveryService.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddSingleton<DiscoveryService.Services.DiscoveryService>();

// File persistence
var envFilePers = Environment.GetEnvironmentVariable("PERSISTENCE_FILE_PATH");
builder.Services.AddSingleton(new DiscoveryFilePersistence(
        envFilePers ?? throw new ArgumentException("Environment variable PERSISTENCE_FILE_PATH is not set.")));

// Logging to console for debugging
builder.Services.AddLogging(options =>
{
    options.AddConsole();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();