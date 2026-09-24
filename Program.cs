using Microsoft.EntityFrameworkCore;
using ToDoListWebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration["SUPABASE_CONNECTION_STRING"]));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapSwagger();
    app.MapSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
