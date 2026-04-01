using Microsoft.AspNetCore.Authentication.Cookies;
using Repositories.Category;
using Repositories.Employee;
using Services.Category;
using Services.Employee;
using Storage;
using Zlagoda.Components;
using Zlagoda.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.Name = "auth_token";
    options.LoginPath = "/login";
    options.Cookie.MaxAge = TimeSpan.FromHours(12);
});
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();


var dbPath = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", "maindb.sqlite"));
builder.Services.AddSingleton<IStorageContext>(_ => new SQLiteStorageContext(dbPath));

builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddSingleton<IEmployeeService, EmployeeService>();
builder.Services.AddSingleton<ICategoryService, CategoryService>();

builder.Services.AddSingleton<CategoriesVM>();
builder.Services.AddSingleton<CustomersVM>();
builder.Services.AddSingleton<EmployeesVM>();
builder.Services.AddSingleton<ProductsVM>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// VERY temporary for testing
var storage = app.Services.GetRequiredService<IStorageContext>();
var user = storage.GetUser(1);

app.Run();