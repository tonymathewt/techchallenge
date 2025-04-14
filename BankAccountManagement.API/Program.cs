using BankAccountManagement.Data;
using BankAccountManagement.Repositories.DependencyInjection;
using BankAccountManagement.Services.DependencyInjection;
using BankAccountManagement.Validators.DependencyInjection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connString = builder.Configuration.GetConnectionString("BankAccountManagment");

builder.Services.AddSqlServer<BankAccountContext>(connString,
    optionsBuilder =>
    {
        optionsBuilder.MigrationsAssembly("BankAccountManagement.Data");
    });

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDataDependencies(connString);
builder.Services.AddServiceDependencies();
builder.Services.AddValidators();

var app = builder.Build();

/// NOTE:- Trigger DB migration on startup; this will fail if DB connnection string is not set!
/// Line 10 in ...\BankAccountManagement.API\appsettings.Development.json
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BankAccountContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
