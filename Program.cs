using bshbbackend;
using Microsoft.EntityFrameworkCore;
using bshbbackend.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BshbDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("bshbContextString")), ServiceLifetime.Scoped);
   builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowOrigin",
            builder => builder
                .WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader());
    });
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<EmailSender>();
builder.Services.AddSingleton<SmsSender>();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowOrigin");
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
