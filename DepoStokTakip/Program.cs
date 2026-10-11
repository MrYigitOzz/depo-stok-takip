using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace DepoStokTakip
{
    internal class Program
    {
        static List<Urun> urunler = new();
        const string DosyaYolu = "urunler.json";
        static readonly JsonSerializerOptions JsonAyarlari = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };

        static void Main(string[] args)
        {
            Yukle();

            int secim = 0;

            while (secim != 6)
            {
                Console.WriteLine("DEPO ÜRÜN STOK TAKİP SİSTEMİ\n1- Ürün Ekle\n2- Ürün Düzenle\n3- Ürün Sil\n4- Ürünleri Listele\n5- Ürün Bul\n6- Çıkış");
                Console.Write("Seçiminizi girin: ");
                secim = SayiOku();
                Console.WriteLine();

                switch (secim)
                {
                    case 1:
                        UrunEkle();
                        break;

                    case 2:
                        if (urunler.Count == 0)
                            Console.WriteLine("Henüz ürün yok!\n");

                        else
                        {
                            string duzenlenenKod = MetinOku("Düzenlemek istediğiniz ürünün kodunu giriniz: ");
                            UrunDuzenle(duzenlenenKod);
                        }

                        break;

                    case 3:
                        if (urunler.Count == 0)
                            Console.WriteLine("Henüz ürün yok!\n");

                        else
                        {
                            string silinenKod = MetinOku("Silmek istediğiniz ürünün kodunu giriniz: ");
                            UrunSil(silinenKod);
                        }

                        break;

                    case 4:
                        if (urunler.Count == 0)
                            Console.WriteLine("Henüz ürün yok!\n");

                        else
                            UrunListele();

                        break;

                    case 5:
                        if (urunler.Count == 0)
                            Console.WriteLine("Henüz ürün yok!\n");

                        else
                        {
                            string arananKod = MetinOku("Bulmak istediğiniz ürünün kodunu girin: ");
                            UrunBul(arananKod);
                        }

                        break;

                    case 6:
                        Console.WriteLine("Çıkış yapılıyor.");
                        break;

                    default:
                        Console.WriteLine("Hatalı seçim yaptınız!\n");
                        break;
                }
            }
        }

        static void UrunEkle()
        {
            Urun urun = new();

            urun.Ad = MetinOku("Ürünün adını girin: ");

            urun.Kod = MetinOku("Ürünün kodunu girin: ");
            while (KodKontrol(urun.Kod))
            {
                Console.WriteLine("Bu kodda ürün zaten var!");
                urun.Kod = MetinOku("Ürünün kodunu girin: ");
            }


            Console.Write("Ürünün kategorisini girin: ");
            urun.Kategori = Console.ReadLine() ?? "";

            int stok;

            do
            {
                Console.Write("Stok sayısı: ");
                stok = SayiOku();
                if (stok < 0)
                    Console.WriteLine("Stok negatif olamaz.");
            } while (stok < 0);

            urun.Stok = stok;

            Console.WriteLine();

            urunler.Add(urun);

            Kaydet();
        }

        static void UrunDuzenle(string kod)
        {
            int secim, stok = 0;

            while (!KodKontrol(kod))
            {
                Console.WriteLine("Bu kodda ürün bulunamadı.");
                kod = MetinOku("Düzenlemek istediğiniz ürünün kodunu girin: ");
            }
            Console.WriteLine("\nÜRÜN BİLGİLERİ");
            UrunBul(kod);

            Console.WriteLine("Düzenleme aşamasına geçmek istiyor musunuz?");
            int cikis = OnayAl();

            if (cikis == 1)
            {
                foreach (var urun in urunler)
                {
                    if (urun.Kod == kod)
                    {
                        do
                        {
                            Console.WriteLine("\nÜrünün hangi bilgisini değiştirmek istiyorsunuz?\n1- Adı\t\t2- Kodu\n3- Kategori\t4- Stok Sayısı");
                            Console.Write("\nSeçiminizi girin: ");
                            secim = SayiOku();

                            if (secim < 1 || secim > 4)
                                Console.WriteLine("Hatalı seçim yaptınız!");

                        } while (secim < 1 || secim > 4);

                        switch (secim)
                        {
                            case 1:
                                urun.Ad = MetinOku("Ürünün yeni adını girin: ");
                                break;

                            case 2:
                                string yeniKod = MetinOku("Ürünün kodunu girin: ");
                                while (KodKontrol(yeniKod))
                                {
                                    Console.WriteLine("Bu kodda ürün zaten var!");
                                    yeniKod = MetinOku("Ürünün kodunu girin: ");
                                }
                                urun.Kod = yeniKod;

                                break;

                            case 3:
                                urun.Kategori = MetinOku("Ürünün yeni kategorisini girin: ");
                                break;

                            case 4:
                                do
                                {
                                    Console.Write("\nÜrünün yeni stok sayısını girin: ");
                                    stok = SayiOku();
                                    if (stok < 0)
                                        Console.WriteLine("Stok sayısı negatif olamaz!");
                                } while (stok < 0);

                                urun.Stok = stok;
                                break;
                        }
                    }
                }

                Console.WriteLine("\nÜrün güncellendi.\n");
                Kaydet();
            }

            else
                Console.WriteLine("\nMenüye geri dönülüyor.\n");
        }

        static void UrunSil(string kod)
        {
            while (!KodKontrol(kod))
            {
                Console.WriteLine("Bu kodda ürün bulunamadı.");
                kod = MetinOku("Silmek istediğiniz ürünün kodunu girin: ");
            }
            Console.WriteLine("\nÜRÜN BİLGİLERİ");
            UrunBul(kod);

            Console.WriteLine("Bu ürünü silmek istediğinize emin misiniz?");
            int secim = OnayAl();

            if (secim == 1)
            {
                foreach (var urun in urunler)
                {
                    if (urun.Kod == kod)
                    {
                        urunler.Remove(urun);
                        break;
                    }
                }
                Console.WriteLine("\nÜrün silindi.\n");
                Kaydet();
            }

            else
                Console.WriteLine("\nMenüye geri dönülüyor.\n");
        }

        static void UrunListele()
        {
            foreach (var urun in urunler)
            {
                Console.WriteLine($"Ürün Adı: {urun.Ad}\nÜrün Kodu: {urun.Kod}\nÜrün Kategorisi: {urun.Kategori}\nÜrün Stok Sayısı: {urun.Stok}\n");
            }
        }

        static string MetinOku(string soru)
        {
            string metin = "";

            while (string.IsNullOrWhiteSpace(metin))
            {
                Console.Write(soru);
                metin = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(metin))
                    Console.WriteLine("Bu alan boş bırakılamaz!");
            }

            return metin.Trim();
        }

        static int SayiOku()
        {
            int sayi;

            while (!int.TryParse(Console.ReadLine(), out sayi))
                Console.Write("Lütfen bir sayı girin: ");

            return sayi;
        }

        static bool KodKontrol(string kod)
        {
            foreach (var urun in urunler)
            {
                if (urun.Kod == kod)
                    return true;
            }

            return false;
        }

        static void UrunBul(string kod)
        {
            bool bul = false;

            foreach (var urun in urunler)
            {
                if (urun.Kod == kod)
                {
                    Console.WriteLine($"Ürün Adı: {urun.Ad}\nÜrün Kodu: {urun.Kod}\nÜrün Kategorisi: {urun.Kategori}\nÜrün Stok Sayısı: {urun.Stok}\n");
                    bul = true;
                }

            }
            if (!bul)
                Console.WriteLine("Bu kodda ürün bulunamadı. Ürün kodunu kontrol edin!\n");
        }

        static int OnayAl()
        {
            int karar;
            Console.WriteLine("0- Hayır       1- Evet");
            do
            {
                Console.Write("Seçiminizi girin: ");
                karar = SayiOku();
                if (karar < 0 || karar > 1)
                    Console.WriteLine("Lütfen geçerli sayılardan birini girin: ");
            } while (karar != 0 && karar != 1);

            return karar;
        }

        static void Yukle()
        {
            if (!File.Exists(DosyaYolu)) return;

            string json = File.ReadAllText(DosyaYolu);
            urunler = JsonSerializer.Deserialize<List<Urun>>(json) ?? new();
        }

        static void Kaydet()
        {
            File.WriteAllText(DosyaYolu, JsonSerializer.Serialize(urunler, JsonAyarlari));
        }
    }
}
