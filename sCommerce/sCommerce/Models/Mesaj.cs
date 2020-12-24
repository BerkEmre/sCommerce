using sCommerce.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sCommerce.Models
{
    public class Mesaj
    {
        public int mesajID;
        public DateTime kayitTarihi;
        public int kaydedenKullaniciID;
        public DateTime guncellemeTarihi;
        public int guncelleyenKullaniciID;
        public int silindi;
        public string adSoyad;
        public string eMail;
        public string telefon;
        public string baslik;
        public string mesaj;
        public string kaynakEkran;
        public int durumParametreID;

        public Mesaj()
        {

        }

        public Mesaj(int mesajID, DateTime kayitTarihi, int kaydedenKullaniciID, DateTime guncellemeTarihi, int guncelleyenKullaniciID, int silindi, string adSoyad, string eMail, string telefon, string baslik, string mesaj, string kaynakEkran, int durumParametreID)
        {
            this.mesajID = mesajID;
            this.kayitTarihi = kayitTarihi;
            this.kaydedenKullaniciID = kaydedenKullaniciID;
            this.guncellemeTarihi = guncellemeTarihi;
            this.guncelleyenKullaniciID = guncelleyenKullaniciID;
            this.silindi = silindi;
            this.adSoyad = adSoyad;
            this.eMail = eMail;
            this.telefon = telefon;
            this.baslik = baslik;
            this.mesaj = mesaj;
            this.kaynakEkran = kaynakEkran;
            this.durumParametreID = durumParametreID;
        }

        public bool Kaydet(string adSoyad, string eMail, string telefon, string baslik, string mesaj, string kaynakEkran)
        {
            SQL.set("INSERT INTO mesajlar (kaydedenKullaniciID, adSoyad, eMail, telefon, baslik, mesaj, kaynakEkran, durumParametreID) VALUES (0, '" + adSoyad + "', '" + eMail + "', '" + telefon + "', '" + baslik + "', '" + mesaj + "', '" + kaynakEkran + "', 5)");
            return true;
        }
    }
}