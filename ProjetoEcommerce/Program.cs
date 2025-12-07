using ProjetoEcommerce.Repositorio;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromHours(2);
});

// DI repositórios
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<UsuarioRepositorio>();
builder.Services.AddScoped<ProdutoRepositorio>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
