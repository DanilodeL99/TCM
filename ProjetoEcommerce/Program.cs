using ProjetoEcommerce.Repositorio;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// INJEÇÃO DE DEPENDENCIA 
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddScoped<UsuarioRepositorio>();
builder.Services.AddScoped<ProdutoRepositorio>();

var app = builder.Build();
// Configure the HTTP request pipeline.
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
