using System.Text;
using DBModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Repositories;
using Repositories.Filiushkin;
using Repositories.Romanyuk;
using Repositories.Category;
using Repositories.CustomerCard;
using Repositories.Employee;
using Repositories.Product;
using Repositories.Receipt;
using Repositories.Sale;
using Repositories.StoreProduct;
using Services.Category;
using Services.Customer;
using Services.Employee;
using Services.Product;
using Services.ProductStore;
using Services.Receipt;
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
builder.Services.AddSingleton<SQLiteStorageContext>(_ => new SQLiteStorageContext(dbPath));

builder.Services.AddSingleton<ICategoryRepository>(sp => sp.GetRequiredService<SQLiteStorageContext>().Categories);
builder.Services.AddSingleton<IEmployeeRepository>(sp => sp.GetRequiredService<SQLiteStorageContext>().Employees);
builder.Services.AddSingleton<ICustomerCardRepository>(sp => sp.GetRequiredService<SQLiteStorageContext>().CustomerCards);
builder.Services.AddSingleton<IProductRepository>(sp => sp.GetRequiredService<SQLiteStorageContext>().Products);
builder.Services.AddSingleton<IStoreProductRepository>(sp => sp.GetRequiredService<SQLiteStorageContext>().StoreProducts);
builder.Services.AddSingleton<IReceiptRepository>(sp => sp.GetRequiredService<SQLiteStorageContext>().Receipts);
builder.Services.AddSingleton<ISaleRepository>(sp => sp.GetRequiredService<SQLiteStorageContext>().Sales);

builder.Services.AddSingleton<IOstapchukQueries>(sp => sp.GetRequiredService<SQLiteStorageContext>().OstapchukQueries);
builder.Services.AddSingleton<IFiliushkinQueries>(sp => sp.GetRequiredService<SQLiteStorageContext>().FiliushkinQueries);
builder.Services.AddSingleton<IRomanyukQueries>(sp => sp.GetRequiredService<SQLiteStorageContext>().RomanyukQueries);

builder.Services.AddSingleton<IEmployeeService, EmployeeService>();
builder.Services.AddSingleton<ICategoryService, CategoryService>();
builder.Services.AddSingleton<IStoreProductService, StoreProductService>();
builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddSingleton<ICustomerService, CustomerService>();
builder.Services.AddSingleton<IReceiptService, ReceiptService>();

builder.Services.AddSingleton<UserInfoVM>();
builder.Services.AddSingleton<CategoriesVM>();
builder.Services.AddSingleton<ProductsInStoreVM>();
builder.Services.AddSingleton<CustomersVM>();
builder.Services.AddSingleton<EmployeesVM>();
builder.Services.AddSingleton<ProductsVM>();
builder.Services.AddSingleton<ReceiptsVM>();
builder.Services.AddSingleton<ProductCountVM>();

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




// TESTING EVERYTHING IN HERE



app.Run();