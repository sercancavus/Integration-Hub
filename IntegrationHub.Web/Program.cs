// IntegrationHub.Web -> Program.cs
using IntegrationHub.Web.Services; // Az önce oluþturduðumuz servisi çaðýrýyoruz

var builder = WebApplication.CreateBuilder(args);

// MVC kullanýyorsan:
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // 30 dakika boþ durursa oturum düþsün
    options.Cookie.HttpOnly = true; // Güvenlik için
    options.Cookie.IsEssential = true; // GDPR kurallarý için gerekli
});

// ÝÞTE BURASI ÖNEMLÝ: ApiService Baðlantýsý
builder.Services.AddHttpClient<ApiService>(client =>
{
    // API PROJENÝN çalýþtýðý adresi buraya yazmalýsýn.
    // IntegrationHub.API projesini çalýþtýrdýðýnda tarayýcýda yazan adres neyse o.
    // Örnek: https://localhost:7045/
    client.BaseAddress = new Uri("http://localhost:5001/");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Oturum yönetimini etkinleþtir

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();