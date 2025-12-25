using IntegrationHub.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// MVC Servisleri
builder.Services.AddControllersWithViews();

// Oturum (Session) Ayarlarý
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpClient<IApiService, ApiService>(client =>
{
    // API'nin launchSettings.json dosyasýndaki adresi buraya yazdýk.
    // DÝKKAT: "https" deðil "http" ve port "5001"
    client.BaseAddress = new Uri("http://localhost:5001/");
});

// ------------------------------------------

var app = builder.Build();

// Hata Yönetimi
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Oturum Middleware'i (Sýralama Önemli: Routing'den sonra, Auth'dan önce)
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();