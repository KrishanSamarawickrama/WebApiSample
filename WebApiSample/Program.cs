using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using WebApiSample.Dtos;
using WebApiSample.Filters;
using WebApiSample.Infrastructure;
using WebApiSample.Models;
using WebApiSample.Persistence;
using WebApiSample.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<HotelInfo>(builder.Configuration.GetSection("Info"));
builder.Services.Configure<HotelOptions>(builder.Configuration);

builder.Services.AddScoped<IRoomService, DefaultRoomService>();
builder.Services.AddScoped<IOpeningService, DefaultOpeningService>();
builder.Services.AddScoped<IBookingService, DefaultBookingService>();
builder.Services.AddScoped<IDateLogicService, DefaultDateLogicService>();

builder.Services.AddDbContext<HotelApiDbContext>(options => options.UseInMemoryDatabase("hotel-db"));

builder.Services.AddControllers(options =>
{
    options.Filters.Add<JsonExceptionFilter>();
    options.Filters.Add<HttpsOrCloseFilter>();
    options.Filters.Add<LinkRewritingFilter>();
});
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddScoped<IRoomService, DefaultRoomService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader = new MediaTypeApiVersionReader();
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
});
builder.Services.AddCors(options => { options.AddPolicy("SamplePolicy", policy => policy.AllowAnyOrigin()); });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("SamplePolicy");
//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await SeedData.InitializeAsync(app);   
MapsterConfig.Configure();

app.Run();