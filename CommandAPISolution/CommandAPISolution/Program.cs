using CommandAPISolution.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddNewtonsoftJson(s =>
{
    s.SerializerSettings.ContractResolver = new
        CamelCasePropertyNamesContractResolver();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICommandAPIRepo, SqlCommandAPIRepo>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



NpgsqlConnectionStringBuilder npgsqlConnectionStringBuilder = new NpgsqlConnectionStringBuilder();
npgsqlConnectionStringBuilder.ConnectionString = builder.Configuration.GetConnectionString("PostgreSqlConnection");
npgsqlConnectionStringBuilder.Username = builder.Configuration["UserID"];
npgsqlConnectionStringBuilder.Password = builder.Configuration["Password"];
    

builder.Services.AddDbContext<CommandContext>(opt =>
    opt.UseNpgsql(npgsqlConnectionStringBuilder.ConnectionString));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.Audience = builder.Configuration["ResourceId"];
        opt.Authority = $"{builder.Configuration["Instance"]}{builder.Configuration["TenantId"]}";
    });
var app = builder.Build();


//var service = app.Services.GetService<CommandContext>();
//service.Database.Migrate();

IServiceScope serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope();

var context = serviceScope.ServiceProvider.GetService<CommandContext>();
context.Database.Migrate();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => "Hello World!");

app.Run();