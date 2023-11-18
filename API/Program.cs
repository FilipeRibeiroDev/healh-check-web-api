using API.HealthChecks;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks()
        .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection"), name: "Health Check Database")
        .AddCheck<YoutubeHealthCheck>("Youtube Health Check");


builder.Services.AddHealthChecksUI(options =>
{
    options.SetEvaluationTimeInSeconds(5);
    options.MaximumHistoryEntriesPerEndpoint(5);
    options.AddHealthCheckEndpoint("API Health Check", "/health");
}).AddInMemoryStorage();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = p => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseHealthChecksUI(options =>
{
    options.UIPath = "/dashboard";
    options.AddCustomStylesheet("./HealthChecks/health-style.css");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
