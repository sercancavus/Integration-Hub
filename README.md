\# 🚀 IntegrationHub - Full-Stack E-Ticaret Yönetim Sistemi



\*\*IntegrationHub\*\*, .NET ekosistemi kullanılarak geliştirilmiş, modern mimariye sahip, uçtan uca (Full-Stack) bir ürün ve kategori yönetim sistemidir. 



Proje, \*\*Web API (Backend)\*\* ve \*\*MVC (Frontend)\*\* olmak üzere iki ana katmandan oluşur ve güvenli veri iletişimi için \*\*JWT (JSON Web Token)\*\* altyapısını kullanır.



---



\## 🏗️ Mimari ve Teknolojiler



Bu proje aşağıdaki teknolojiler ve prensipler kullanılarak geliştirilmektedir:



\* \*\*Backend:\*\* ASP.NET Core Web API (.NET 8)

\* \*\*Frontend:\*\* ASP.NET Core MVC \& Razor Views

\* \*\*Veritabanı:\*\* MS SQL Server \& Entity Framework Core (Code-First)

\* \*\*Kimlik Doğrulama:\*\* JWT (JSON Web Token) \& Identity Library

\* \*\*Tasarım:\*\* Bootstrap 5 \& CSS

\* \*\*Dokümantasyon:\*\* Swagger / OpenAPI

\* \*\*Versiyon Kontrol:\*\* Git \& GitHub



---



\## 🔥 Temel Özellikler



\### 🔐 Kimlik Doğrulama ve Güvenlik

\* \*\*Kullanıcı Kaydı (Register):\*\* Yeni kullanıcı oluşturma.

\* \*\*Giriş (Login):\*\* JWT tabanlı güvenli oturum açma.

\* \*\*Oturum Yönetimi:\*\* Session bazlı token saklama ve otomatik çıkış (Logout).

\* \*\*Korunan Sayfalar:\*\* Giriş yapmamış kullanıcıların yönetim paneline erişiminin engellenmesi.



\### 📦 Ürün ve Kategori Yönetimi (CRUD)

\* \*\*Listeleme:\*\* Ürünleri ilişkili oldukları kategorilerle birlikte listeleme.

\* \*\*Ekleme:\*\* Dinamik kategori seçimi (Dropdown) ile yeni ürün ekleme.

\* \*\*Güncelleme:\*\* Mevcut ürün bilgilerini düzenleme formları.

\* \*\*Silme:\*\* Onay mekanizması ile güvenli veri silme.

\* \*\*İlişkisel Yapı:\*\* One-to-Many (Bir Kategori - Çok Ürün) veritabanı ilişkisi.



\### 🎨 Arayüz (UI/UX)

\* \*\*Dinamik Menü:\*\* Kullanıcının giriş durumuna göre değişen (Login/Logout) akıllı Navbar.

\* \*\*Responsive Tasarım:\*\* Mobil uyumlu Bootstrap tabloları ve formları.

\* \*\*Hata Yönetimi:\*\* Kullanıcı dostu hata mesajları ve yönlendirmeler.



---

👨‍💻 Geliştirici

Sercan Çavuş - GitHub: github.com/sercancavus

---

Bu proje öğrenme ve portföy geliştirme amacıyla açık kaynak olarak sunulmuştur.



\## 🚀 Kurulum ve Çalıştırma



Projeyi yerel makinenizde çalıştırmak için adımları izleyin:



\### 1. Projeyi Klonlayın

```bash

git clone \[https://github.com/sercancavus/Integration-Hub.git](https://github.com/sercancavus/Integration-Hub.git)

cd Integration-Hub





