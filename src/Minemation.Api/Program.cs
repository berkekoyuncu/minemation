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
builder.Services.AddDbContext<MinemationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Dependency Injection kayıtları
builder.Services.AddScoped<IPersonelServisi, PersonelServisi>();

// ŞİMDİLİK FAKE REPOSITORY KULLANIYORUZ.
// Gerçek veritabanı ile test edeceğimiz zaman bu satırı kapatıp alttaki PersonelRepository satırını açacağız.
//builder.Services.AddScoped<IPersonelRepository, SahtePersonelRepository>();

// GERÇEK VERİTABANINA GEÇİNCE BUNU AÇ:
builder.Services.AddScoped<IPersonelRepository, PersonelRepository>();

builder.Services.AddScoped<IEkipmanRepository, EkipmanRepository>();
builder.Services.AddScoped<IEkipmanServisi, EkipmanServisi>();

builder.Services.AddScoped<IVardiyaRepository, VardiyaRepository>();
builder.Services.AddScoped<IVardiyaServisi, VardiyaServisi>();

builder.Services.AddScoped<IVakaRepository, VakaRepository>();
builder.Services.AddScoped<IVakaServisi, VakaServisi>();

builder.Services.AddScoped<IAksiyonServisi, AksiyonServisi>();
builder.Services.AddScoped<IAksiyonRepository, AksiyonRepository>();

builder.Services.AddScoped<IEkipServisi, EkipServisi>();
builder.Services.AddScoped<IEkipRepository, EkipRepository>();

builder.Services.AddScoped<ISensorServisi, SensorServisi>();
builder.Services.AddScoped<ISensorRepository, SensorRepository>();

builder.Services.AddScoped<ISensorVerisiServisi, SensorVerisiServisi>();
builder.Services.AddScoped<ISensorVerisiRepository, SensorVerisiRepository>();

builder.Services.AddScoped<ITakipCihaziServisi, TakipCihaziServisi>();
builder.Services.AddScoped<ITakipCihaziRepository, TakipCihaziRepository>();

builder.Services.AddScoped<IRaporServisi, RaporServisi>();
builder.Services.AddScoped<IRaporRepository, RaporRepository>();

builder.Services.AddScoped<IVakaRaporuServisi, VakaRaporuServisi>();
builder.Services.AddScoped<IVakaRaporuRepository, VakaRaporuRepository>();

builder.Services.AddScoped<IPersonelRaporuServisi, PersonelRaporuServisi>();
builder.Services.AddScoped<IPersonelRaporuRepository, PersonelRaporuRepository>();

builder.Services.AddScoped<IEkipmanRaporuServisi, EkipmanRaporuServisi>();
builder.Services.AddScoped<IEkipmanRaporuRepository, EkipmanRaporuRepository>();

builder.Services.AddScoped<ILogKaydiServisi, LogKaydiServisi>();
builder.Services.AddScoped<ILogKaydiRepository, LogKaydiRepository>();

builder.Services.AddScoped<IHafriyatServisi, HafriyatServisi>();
builder.Services.AddScoped<IHafriyatRepository, HafriyatRepository>();


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