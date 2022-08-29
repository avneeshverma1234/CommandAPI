using CommandAPISolution.Data;
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

var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.MapControllers();
app.MapGet("/", () => "Hello World!");

app.Run();