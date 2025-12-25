# IntegrationHub 🛒

IntegrationHub, .NET 8 teknolojileri kullanılarak geliştirilmiş, N-Katmanlı mimariye sahip bir E-Ticaret ve Stok Yönetim sistemidir. Proje, RESTful API servisleri ve bu servisleri tüketen bir MVC Web arayüzünden oluşur.

## 🚀 Teknolojiler

- **Backend:** .NET 8 Web API, Entity Framework Core
- **Frontend:** ASP.NET Core MVC, Bootstrap 5
- **Veritabanı:** MSSQL (Code-First)
- **Güvenlik:** JWT (JSON Web Token) Auth
- **Versiyon Kontrol:** Git & GitHub

## ⚙️ Özellikler

- **Admin Paneli:**
  - Güvenli Giriş Sistemi (Login/Register)
  - Ürün Yönetimi (Ekle/Sil/Güncelle/Listele)
  - Kategori Yönetimi
  - Resim Yükleme (Image Upload)
- **Vitrin (Storefront):**
  - Tüm kullanıcılar için ürün listeleme (Public Access)
  - Dinamik ürün kartları ve stok durumu görüntüleme

## 🛠️ Kurulum

1. Projeyi klonlayın.
2. `appsettings.json` dosyasındaki ConnectionString'i kendi SQL sunucunuza göre düzenleyin.
3. Package Manager Console üzerinden `Update-Database` komutunu çalıştırın.
4. Önce API projesini, ardından Web projesini çalıştırın.
