# Depo Ürün Stok Takip Sistemi

C# ile yazılmış bir konsol uygulaması: depodaki ürünleri ekler, düzenler, siler, listeler ve ürün koduna göre arar. Veriler bir JSON dosyasında saklanır; program kapatılıp yeniden açıldığında stok kayıtları kaldığı yerden devam eder.

## Özellikler

- **Ürün ekleme:** Ad, benzersiz ürün kodu, kategori ve stok sayısı kaydedilir.
- **Ürün düzenleme:** Koduna göre seçilen ürünün adı, kodu, kategorisi veya stok sayısı güncellenir (önce onay istenir).
- **Ürün silme:** Ürün bilgileri gösterilir, onay alındıktan sonra kayıt silinir.
- **Listeleme ve arama:** Tüm ürünler listelenir; tek bir ürün koduna göre bulunabilir.
- **Girdi doğrulama:** Boş alanlar, aynı kodlu ikinci ürün, negatif stok ve sayı beklenen yere girilen harfler engellenir.
- **Kalıcı kayıt:** Veriler her değişiklikten sonra `urunler.json` dosyasına yazılır ve program açılırken otomatik yüklenir.

## Başlarken

**Gereksinim:** [.NET SDK](https://dotnet.microsoft.com/download) 6.0 veya üzeri (projenin hedeflediği sürüm `.csproj` dosyasındaki `TargetFramework` alanında yazar).

**Terminalden çalıştırma** — `.csproj` dosyasının bulunduğu klasörde:

```bash
dotnet run
```

**Visual Studio ile:** `.sln` dosyasını açıp `Ctrl + F5` ile çalıştırın.

## Kullanım

Program açılınca ana menü görünür:

```text
DEPO ÜRÜN STOK TAKİP SİSTEMİ
1- Ürün Ekle
2- Ürün Düzenle
3- Ürün Sil
4- Ürünleri Listele
5- Ürün Bul
6- Çıkış
Seçiminizi girin:
```

| Seçim | İşlem | Ne yapar? |
|:-----:|-------|-----------|
| `1` | Ürün Ekle | Yeni ürün ekler (ad, kod, kategori, stok) |
| `2` | Ürün Düzenle | Kodu girilen ürünün bir bilgisini değiştirir |
| `3` | Ürün Sil | Kodu girilen ürünü onay alarak siler |
| `4` | Ürünleri Listele | Kayıtlı tüm ürünleri gösterir |
| `5` | Ürün Bul | Kodu girilen ürünün bilgilerini gösterir; bulunamazsa uyarır |
| `6` | Çıkış | Programdan çıkar |

Hiç ürün kayıtlı değilken `2`, `3`, `4` ve `5` seçilirse `Henüz ürün yok!` uyarısı gösterilir. Menüde bunlar dışında bir sayı girilirse `Hatalı seçim yaptınız!` yazılır.

### Ürün ekleme

Depoda `CK-001` kodlu bir ürün (Çekiç) varken yeni ürün ekleme:

```text
Seçiminizi girin: 1

Ürünün adını girin: Tornavida
Ürünün kodunu girin: CK-001
Bu kodda ürün zaten var!
Ürünün kodunu girin: TV-002
Ürünün kategorisini girin: El Aletleri
Stok sayısı: -5
Stok negatif olamaz.
Stok sayısı: 40
```

### Ürün düzenleme

```text
Seçiminizi girin: 2

Düzenlemek istediğiniz ürünün kodunu giriniz: CK-001

ÜRÜN BİLGİLERİ
Ürün Adı: Çekiç
Ürün Kodu: CK-001
Ürün Kategorisi: El Aletleri
Ürün Stok Sayısı: 18

Düzenleme aşamasına geçmek istiyor musunuz?
0- Hayır       1- Evet
Seçiminizi girin: 1

Ürünün hangi bilgisini değiştirmek istiyorsunuz?
1- Adı          2- Kodu
3- Kategori     4- Stok Sayısı

Seçiminizi girin: 4

Ürünün yeni stok sayısını girin: 25

Ürün güncellendi.
```

- Her düzenlemede **tek bir alan** değiştirilir; birden fazla alan için işlemi tekrarlayın.
- Kod değiştirilirse yeni kod da benzersiz olmalıdır.
- Onay sorusunda `0` girilirse hiçbir şey değişmeden menüye dönülür.
- **Ürün silme** de aynı akışı izler: kod sorulur, ürün bilgileri gösterilir, `0- Hayır / 1- Evet` onayı alınır; onaylanırsa ürün silinir ve `Ürün silindi.` yazılır.

### Listeleme

Yukarıdaki işlemlerin ardından:

```text
Seçiminizi girin: 4

Ürün Adı: Çekiç
Ürün Kodu: CK-001
Ürün Kategorisi: El Aletleri
Ürün Stok Sayısı: 25

Ürün Adı: Tornavida
Ürün Kodu: TV-002
Ürün Kategorisi: El Aletleri
Ürün Stok Sayısı: 40
```

### Girdi kuralları

- Menü seçimi, stok sayısı ve onay cevabı **tam sayı** olmalıdır; aksi hâlde `Lütfen bir sayı girin:` uyarısıyla yeniden sorulur.
- Ürün adı ve kodu **boş bırakılamaz** (`Bu alan boş bırakılamaz!`); baştaki ve sondaki boşluklar otomatik silinir.
- Ürün kodu **benzersiz** olmalıdır ve büyük/küçük harfe duyarlıdır (`ck-001` ile `CK-001` farklı kodlardır). Kontrol, ürün eklenirken ve kod düzenlenirken yapılır.
- Stok sayısı **negatif olamaz** (`0` geçerlidir).
- Onay sorularında yalnızca `0` (Hayır) veya `1` (Evet) kabul edilir.
- Ürün arama yalnızca **koda göre** yapılır ve kod tam olarak eşleşmelidir.

## Veri Saklama

Ürünler `urunler.json` dosyasında girintili (okunabilir) JSON olarak tutulur:

```json
[
  {
    "Ad": "Çekiç",
    "Kod": "CK-001",
    "Kategori": "El Aletleri",
    "Stok": 25
  },
  {
    "Ad": "Tornavida",
    "Kod": "TV-002",
    "Kategori": "El Aletleri",
    "Stok": 40
  }
]
```

- Dosya her ekleme, düzenleme ve silme işleminden sonra güncellenir; program açılırken (varsa) otomatik okunur.
- Dosya yolu göreli (`urunler.json`) olduğundan dosya programın çalışma klasöründe oluşur: terminalde `dotnet run` komutunu verdiğiniz klasör, Visual Studio'da genellikle `bin/Debug/net.../` klasörü.
- Türkçe karakterler kaçış dizisine çevrilmeden olduğu gibi yazılır.
- Kayıtları sıfırlamak için `urunler.json` dosyasını silmeniz yeterlidir.
- Dosyayı elle düzenlerseniz JSON biçimini ve ürün kodlarının benzersizliğini bozmamaya dikkat edin; geçersiz bir dosya programın açılışta hata vermesine yol açar.

## Kod Yapısı

Başlıca dosyalar:

```text
.
├── Program.cs     # Menü, ürün işlemleri, girdi okuma, JSON kaydetme/yükleme
├── Urun.cs        # Ürün sınıfı (Ad, Kod, Kategori, Stok)
└── urunler.json   # Çalışırken otomatik oluşur
```

| Dosya | Üye | Görevi |
|-------|-----|--------|
| `Program.cs` | `Main` | Menüyü gösterir, seçime göre ilgili metodu çağırır; `6` seçilene kadar döngüde kalır |
| | `UrunEkle()` | Ad, benzersiz kod, kategori ve stok bilgisini alıp ürünü ekler ve kaydeder |
| | `UrunDuzenle(kod)` | Ürünü bulur, onay alır, seçilen alanı günceller ve kaydeder |
| | `UrunSil(kod)` | Ürünü bulur, onay alır, listeden kaldırır ve kaydeder |
| | `UrunListele()` / `UrunBul(kod)` | Tüm ürünleri listeler / koda göre tek ürün gösterir |
| | `KodKontrol(kod)` | Kodun listede olup olmadığını döndürür (benzersizlik kontrolü) |
| | `MetinOku(soru)` | Boş olmayan bir metin girilene kadar sorar, kırpılmış metni döndürür |
| | `SayiOku()` | Geçerli bir tam sayı girilene kadar tekrar sorar (`int.TryParse`) |
| | `OnayAl()` | `0- Hayır / 1- Evet` onayı alır |
| | `Yukle()` / `Kaydet()` | `urunler.json` dosyasını `System.Text.Json` ile okur / yazar |
| `Urun.cs` | `Urun` | `Ad`, `Kod`, `Kategori` ve `Stok` özelliklerini tutan model sınıfı |

Bu projede kullanılan başlıca kavramlar: sınıf ve özellikler (property), `List<T>`, `switch` / `do-while` / `while` / `foreach` yapıları, tekrar kullanılabilir yardımcı metotlar (`MetinOku`, `SayiOku`, `OnayAl`), `string.IsNullOrWhiteSpace` ve `Trim()` ile girdi temizleme, `int.TryParse` ile doğrulama, `System.Text.Json` ve `JsonSerializerOptions` (girintili çıktı, Türkçe karakter desteği) ile dosya işlemleri.
