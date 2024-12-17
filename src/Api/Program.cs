using Api.Database;
using Api.UseCases;
using Api.Workers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseAccess(builder.Configuration);
builder.Services.AddMailing(builder.Configuration);
builder.Services.AddStreamElementsClient(builder.Configuration);
builder.Services.AddUseCases();
builder.Services.AddWorkers();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dbConnectionString =
    builder.Configuration.GetConnectionString("Database");

builder.Services.AddHealthChecks().AddNpgSql(dbConnectionString!);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers().WithOpenApi();
app.UseHealthChecks("/health");

await app.RunAsync();
