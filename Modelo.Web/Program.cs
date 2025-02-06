using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Modelo.Dominio.DTO;
using Modelo.Infraestrutura.CrossCutting.IoC;
using Modelo.Web.Configuracoes.AutoMapper;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.Configure<EmailConfiguracao>(builder.Configuration.GetSection("EmailConfiguracao"));

InjetorDependencias.ConfigurarContextosEFCore(builder.Services, connectionString);

//InjetorDependencias.ConfigurarIdentity(builder.Services);
InjetorDependencias.ConfigurarServicosERepositorios(builder.Services);

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
}).AddAuthentication(o =>
{
    o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(

    CookieAuthenticationDefaults.AuthenticationScheme, (options) =>
    {
        options.LoginPath = "/Usuarios/Login";
        options.LogoutPath = "/Usuarios/Logout";
    }
);

// Add services to the container.
InjetorDependencias.ConfigurarMensagensMVC(builder.Services);


builder.Services.AddCors(o =>
{
    o.AddDefaultPolicy(p =>
    {
        p.AllowAnyHeader();
        p.AllowAnyMethod();
        p.AllowAnyOrigin();
    });
});
builder.Services.AddResponseCaching();
builder.Services.Configure<FormOptions>(x =>
{
    x.ValueCountLimit = int.MaxValue;
});

InjetorDependencias.ConfigurarAutoMapper(builder.Services);
builder.Services.AddAutoMapper(typeof(MapearViewModelParaDTO), typeof(MapearDTOParaViewModel));

var app = builder.Build();

var supportedCultures = new[] { new CultureInfo("pt-BR") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseResponseCaching();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();


