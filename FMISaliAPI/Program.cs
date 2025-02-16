using System.Net.Mime;
using FMISaliAPI.Data;
using FMISaliAPI.Services.EmailService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IEmailSender>(provider =>
    {
        try
        {
            var smtpServer = builder.Configuration["Smtp:Server"];
            var smtpPort = int.Parse(builder.Configuration["Smtp:Port"] ?? "-1");
            var smtpUsername = builder.Configuration["Smtp:Username"];
            var smtpPassword = builder.Configuration["Smtp:Password"];
            if (string.IsNullOrEmpty(smtpServer) || string.IsNullOrEmpty(smtpUsername) ||
                string.IsNullOrEmpty(smtpPassword) || smtpPort == -1)
            {
                throw new Exception("SMTP server, port, username, and password must be provided in the configuration.");
            }

            return new EmailSender(smtpServer, smtpPort, smtpUsername, smtpPassword);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Environment.Exit(1);
        }
        return null;
    }
);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactFrontEnd", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Ensure the database is created and migrated
    dbContext.Database.Migrate();

    // Seed schedules
    var seeder = new RoomScheduleSeed(dbContext);
    seeder.SeedSchedules();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactFrontEnd");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
