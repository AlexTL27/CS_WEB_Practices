using Microsoft.EntityFrameworkCore;
using WebChat.Controllers;
using WebChat.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configuracion para entity framework
var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

//Configuracion para SignalR
builder.Services.AddSignalR();

//Configuracion para cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicyAll", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500");
        
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
        policy.AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicyAll");

app.UseHttpsRedirection();
app.MapHub<ChatHub>("/chat");

app.MapControllers();
app.Run();



