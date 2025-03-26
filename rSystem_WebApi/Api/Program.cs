using Application;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");
// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
ServiceRegister.RegisterServices(builder.Services, builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")  // Allow your frontend's origin
              .AllowAnyHeader()                       // Allow any headers
              .AllowAnyMethod();                       // Allow any HTTP methods
    });
});
builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; })
    .AddXmlSerializerFormatters(); //Enabling XML support

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
//Middle wares added
app.UseHttpsRedirection();
app.UseExceptionHandler("/Error");
app.UseHttpsRedirection();
app.UseAuthentication();//Todo:We can configure later for JWT
app.UseAuthorization();
app.UseCors("AllowFrontend");//Todo:We can configure later
app.MapControllers();

app.Run();
