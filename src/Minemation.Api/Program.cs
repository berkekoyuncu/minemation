using Microsoft.EntityFrameworkCore;
using Minemation.Api.Middlewares;
using Minemation.Application.Interfaces;
using Minemation.Application.Services;
using Minemation.Infrastructure.Persistence;
using Minemation.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Veritabanı bağlantısı
// Şimdilik fake repository kullansak bile DbContext kaydı durabilir.
// Gerçek repository'ye geçince zaten gerekli olacak.
builder.Services.AddDbContext<MinemationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Dependency Injection kayıtları
builder.Services.AddScoped<IPersonelServisi, PersonelServisi>();

// ŞİMDİLİK FAKE REPOSITORY KULLANIYORUZ
builder.Services.AddScoped<IPersonelRepository, SahtePersonelRepository>();

// GERÇEK VERİTABANINA GEÇİNCE ÜSTTEKİ SAHTE KAYDI KAPATIP BUNU AÇ:
// builder.Services.AddScoped<IPersonelRepository, PersonelRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Ana adrese gidince Swagger'a yönlendirir
app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();