using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Set up routes discovery
app.MapGet("/discovery/routes", (IEnumerable<EndpointDataSource> sources) =>
{
    return string.Join("\n", sources.SelectMany(source => source.Endpoints));
});

// Dummy route that performs computation
app.MapGet("/RequestWork", async () => {
        // SHA512 instance
        SHA512 sha512 = SHA512.Create();
        sha512.Initialize();
        
        // Create hashes from random data
        List<string> hashes = new List<string>();
        for (int i = 0; i < 2048; i++)
        {
            var hash = await sha512.ComputeHashAsync(new MemoryStream(RandomNumberGenerator.GetBytes(64)));
            hashes.Add(Convert.ToBase64String(hash));
        }
        
        // Return base64 hashes
        return hashes;
    })
    .WithName("RequestWork")
    .WithOpenApi();

app.Run();