
# 🎯 CareerSim

CareerSim, yapay zekâ destekli bir **kariyer değerlendirme simülatörüdür**. Kullanıcıların verdiği yanıtlara göre kişisel özelliklerini analiz eder, uygun meslekleri önerir ve detaylı değerlendirmeler sunar.

> Proje, ASP.NET Core MVC altyapısı üzerinde inşa edilmiş; AI değerlendirmeleri ise OpenAI API ile gerçekleştirilmiştir.

---

## 🔍 Özellikler

- 🧠 **Yapay Zekâ Destekli Değerlendirme**  
  OpenAI üzerinden gönderilen veriler doğrultusunda, adayın yetkinlikleri analiz edilir.

- 🧾 **Soru Sistemi ve Meslek Profilleri**  
  Admin panelden yönetilebilen soru ve meslek yapısı.

- 🖥️ **Modern Yönetim Paneli (AdminPanel Entegre)**  
  Meslek, soru, cevap ve AI sonuçları detaylı olarak yönetilebilir.

- 💡 **Dinamik AI Prompt Sistemi**  
  Verilen cevaplara göre anlık olarak prompt oluşturulur ve değerlendirme alınır.

---

## 🛠️ Kullanılan Teknolojiler

| Katman | Teknoloji |
|--------|-----------|
| Backend | ASP.NET Core MVC |
| Veritabanı | Entity Framework, MSSQL |
| Frontend | Razor View + Custom CSS |
| AI Entegrasyonu | OpenAI GPT-4 API |
| UI | AdminPanel yapısı üzerinden özelleştirildi |

---

## 🖼️ Ekran Görüntüsü

![career_sim_screenshot](https://github.com/erdokrmn/CareerSim/assets/preview.png) <!-- İstersen buraya ekran görüntüsü yükleyebilirsin -->

---

## 🚀 Kurulum

```bash
git clone https://github.com/erdokrmn/CareerSim.git
cd CareerSim
```

### 📦 Gerekli Paketler

- .NET 6 SDK
- Visual Studio 2022+ (MVC ve EF desteğiyle)
- MSSQL Server (veya LocalDB)

---

## ▶️ Başlatmak için

1. `appsettings.json` dosyasına OpenAI API anahtarını ekle:

```json
"OpenAI": {
  "ApiKey": "YOUR_OPENAI_KEY"
}
```

2. Veritabanını oluştur:
   - `dotnet ef database update`

3. Uygulamayı çalıştır:
   - `dotnet run` veya Visual Studio üzerinden `IIS Express`

---

## 🧠 Örnek AI Cevabı

```txt
Değerlendirme: Analitik düşünebilen, problem çözme becerileri gelişmiş bir bireysiniz. Yazılım geliştirme, veri analizi gibi teknik alanlara yatkınsınız.
Önerilen meslekler: Yazılım Mühendisi, Veri Bilimci, Sistem Analisti
```

---

## 📌 Katkıda Bulunmak

Katkılarınızı memnuniyetle karşılıyorum! Lütfen:

- Forklayın
- Geliştirme yapın
- Pull Request gönderin

---

## 📄 Lisans

MIT Lisansı – Dilediğiniz gibi kullanabilir ve geliştirebilirsiniz.
