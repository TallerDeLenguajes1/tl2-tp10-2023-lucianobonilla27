using tl2_tp09_2023_lucianobonilla27.Models;
using tl2_tp09_2023_lucianobonilla27.Repository;
using tl2_tp10_2023_lucianobonilla27.Repository;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor(); // Agregado
builder.Services.AddDistributedMemoryCache();

var CadenaDeConexion = builder.Configuration.GetConnectionString("SqliteConexion")!;
builder.Services.AddSingleton<string>(CadenaDeConexion);


builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ITableroRepository,TableroRepository>();
builder.Services.AddScoped<ITareaRepository,TareaRepository>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(300);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();