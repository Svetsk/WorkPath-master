using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WorkPath.Server;
using WorkPath.Server.Helpers;
using WorkPath.Server.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("AUTH_CONNECTION_STRING") ??
                       "Host=portainer.main.kaboom.pro;Database=workpath;Username=postgres;Password=devversionsuck";

builder.Services.AddDbContext<ServerContext>(options => options.UseNpgsql(connectionString));

#region Auth

// Adding Authentification helper.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => { options.TokenValidationParameters = AuthService.BuilderParamethers; });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserEntity", policy => { policy.RequireClaim("Entity", "User"); });
});

#endregion


// Add services to the container.
builder.Services.AddTransient<AuthService>();
builder.Services.AddTransient<CompanyService>();
builder.Services.AddTransient<EducationService>();
builder.Services.AddTransient<JobService>();
builder.Services.AddTransient<UserService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v" + 1,
        SwaggerHelper.ApiInformation);
    
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    
    options.AddSecurityDefinition("JWT Token", SwaggerHelper.ApiSecurityDefinition);

    options.AddSecurityRequirement(SwaggerHelper.ApiSecurityRequirement);
});

builder.Services.AddAutoMapper(typeof(Mappings));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

// Create DB.

var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<ServerContext>();
await db.Database.EnsureCreatedAsync();

app.Run();