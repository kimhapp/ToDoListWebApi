using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ToDoListWebApi;
using ToDoListWebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration["SUPABASE_CONNECTION_STRING"]));

builder.Services.AddAuthentication().AddJwtBearer(jwtOptions =>
{
    jwtOptions.TokenValidationParameters = new() {
        ValidateIssuer = true,
        ValidIssuers = [builder.Configuration["WebApiName"]],

        ValidateAudience = true,
        ValidAudiences = [builder.Configuration["WebApiName"]],

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwtKey"]!)),
        ValidateLifetime = true,
    };

    jwtOptions.MapInboundClaims = false;
});

builder.Services.AddScoped<IToDoService, TodoService>();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
