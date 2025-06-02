using ProjectPlanner_API;
using ProjectPlanner_API.IMethod;
using ProjectPlanner_API.Repository;

var builder = WebApplication.CreateBuilder(args);

// Allow CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhostFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:57018") // your frontend port
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddSingleton(new ConnectionStringProvider(builder.Configuration.GetConnectionString("conn").ToString()));
builder.Services.AddScoped<IAccount, AccountRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowLocalhostFrontend");
app.UseAuthorization();

app.MapControllers();

app.Run();
