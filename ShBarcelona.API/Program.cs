using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NLog.Web;
using ShBarcelona.DAL;
using ShBarcelona.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Host.UseNLog();

// Add services to the container.
builder.Services.AddServices(builder.Configuration);
builder.Services.AddDAL(builder.Configuration);
builder.Services.AddControllers().AddNewtonsoftJson(options =>
                            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(x =>
{
    // TODO: Review Cors policies
    x.AddPolicy("All", builder => builder.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader());
    x.AddPolicy("CorsPolicy", builder => builder
                                .WithOrigins("http://localhost:4200")
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    app.UseCors("CorsPolicy");
}
else
{
    app.UseCors("All");
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// migrate any database changes on startup (includes initial db creation)
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<ShBarcelonaContext>();
    dataContext.Database.Migrate();
}

app.Run();