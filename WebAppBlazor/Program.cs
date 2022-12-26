using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using WebAppBlazor.Data;
using WebAppBlazor.Services;
using static System.Net.WebRequestMethods;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddHttpClient<IPassesServiceA, PassesServiceA>( x =>
{
    x.BaseAddress = new Uri("https://localhost:7057/api/");
    x.DefaultRequestHeaders.Add("User-Agent", "BlazorServer");
});
builder.Services.AddHttpClient<IPassesServiceB, PassesServiceB>( x =>
{
    x.BaseAddress = new Uri("https://localhost:7106/api/");
    x.DefaultRequestHeaders.Add("User-Agent", "BlazorServer");
});

builder.Services.AddHttpClient<IAppointmentCurrentService, AppointmentCurrentService>( x =>
{
    x.BaseAddress = new Uri("https://localhost:7057/api/AppointmentCurrents/");
    x.DefaultRequestHeaders.Add("User-Agent", "BlazorServer");
});
builder.Services.AddHttpClient<IRecordsCurrentService, RecordsCurrentService>(x =>
{
    x.BaseAddress = new Uri("https://localhost:7106/api/RecordCurrents/");
    x.DefaultRequestHeaders.Add("User-Agent", "BlazorServer");
});
builder.Services.AddHttpClient<ISpecialistService, SpecialistService>(x =>
{
    x.BaseAddress = new Uri("https://localhost:7057/api/");
    x.DefaultRequestHeaders.Add("User-Agent", "BlazorServer");
});
builder.Services.AddHttpClient<IAreaService, AreaService>(x =>
{
    x.BaseAddress = new Uri("https://localhost:7106/api/");
    x.DefaultRequestHeaders.Add("User-Agent", "BlazorServer");
});
builder.Services.AddHttpClient<IRoomService, RoomService>(x =>
{
    x.BaseAddress = new Uri("https://localhost:7106/api/Rooms/");
    x.DefaultRequestHeaders.Add("User-Agent", "BlazorServer");
});

builder.Services.AddSingleton<HttpClient>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
