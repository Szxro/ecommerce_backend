using Application;
using Infrastructure;
using Web_Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.

    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddInfrastructureAuthentication(builder.Configuration);
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.ConfigureSwagger();
    builder.Configuration.AddUserSecrets<Program>(optional: false, reloadOnChange: true); // Adding the user secrets
}


var app = builder.Build();
{ 
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        await app.InitializeDatabaseAsync(); // Migrating and Seeding if its necessary
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}

