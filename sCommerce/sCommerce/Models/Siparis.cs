using System;
using System.Collections.Generic;
using sCommerce.Helper;
using sCommerce.Helper;
using System.Web;
using System.Data;
using System.Linq;

namespace sCommerce.Models
{
    public class Siparis
    {
        public int siparisID;
        public DateTime kayitTarihi;
        public int kaydedenKullaniciID;
        public DateTime guncellemeTarihi;
        public int guncelleyenKullaniciID;
        public int silindi;
        public string ad;
        public string soyad;
        public string telefon;
        public string sehir;
        public string semt;
        public string mahalle;
        public string postaKodu;
        public string adresSatir1;
        public string adresSatir2;
        public int kullaniciID;
        public int siparisDurum;
        public int odemeTipi;
        public string siparisNotu;
        public string kargoNo;
        public List<SiparisKalem> siparisKalemleri;

        public string GetSiparisDurum()
        {
            if (this.siparisDurum == Convert.ToInt32(Helper.siparisDurum.hazirlaniyor))
            {
                return "Hazırlanıyor";
            }
            else if (this.siparisDurum == Convert.ToInt32(Helper.siparisDurum.iptalEdildi))
            {
                return "İptal Edildi";
            }
            else if (this.siparisDurum == Convert.ToInt32(Helper.siparisDurum.kargoda))
            {
                return "Kargoda";
            }
            else if (this.siparisDurum == Convert.ToInt32(Helper.siparisDurum.odemeBasarisiz))
            {
                return "Ödeme Başarısız";
            }
            else if (this.siparisDurum == Convert.ToInt32(Helper.siparisDurum.odemeBekliyor))
            {
                return "Ödeme Bekliyor";
            }
            else if (this.siparisDurum == Convert.ToInt32(Helper.siparisDurum.teslimEdildi))
            {
                return "Teslim Edildi";
            }
            else
                return "---";
        }

        public decimal GetToplamFiyat()
        {
            return siparisKalemleri.Sum(x => x.miktar * x.fiyat);
        }

        public decimal GetToplamAdet()
        {
            return siparisKalemleri.Sum(x => x.miktar);
        }

        public bool Load(int siparisID)
        {
            DataTable dt = SQL.get("SELECT * FROM siparisler WHERE silindi = 0 AND siparisID = " + siparisID);
            if (dt.Rows.Count <= 0)
                return false;

            DataRow dr = dt.Rows[0];

            Int32.TryParse(dr["siparisID"].ToString(), out this.siparisID);
            DateTime.TryParse(dr["kayitTarihi"].ToString(), out this.kayitTarihi);
            Int32.TryParse(dr["kaydedenKullaniciID"].ToString(), out this.kaydedenKullaniciID);
            DateTime.TryParse(dr["guncellemeTarihi"].ToString(), out this.guncellemeTarihi);
            Int32.TryParse(dr["guncelleyenKullaniciID"].ToString(), out this.guncelleyenKullaniciID);
            Int32.TryParse(dr["silindi"].ToString(), out this.silindi);
            this.ad = dr["ad"].ToString();
            this.soyad = dr["soyad"].ToString();
            this.telefon = dr["telefon"].ToString();
            this.sehir = dr["sehir"].ToString();
            this.semt = dr["semt"].ToString();
            this.mahalle = dr["mahalle"].ToString();
            this.postaKodu = dr["postaKodu"].ToString();
            this.adresSatir1 = dr["adresSatir1"].ToString();
            this.adresSatir2 = dr["adresSatir2"].ToString();
            Int32.TryParse(dr["kullaniciID"].ToString(), out this.kullaniciID);
            Int32.TryParse(dr["siparisDurum"].ToString(), out this.siparisDurum);
            Int32.TryParse(dr["odemeTipi"].ToString(), out this.odemeTipi);
            this.siparisNotu = dr["siparisNotu"].ToString();
            this.kargoNo = dr["kargoNo"].ToString();
            this.siparisKalemleri = new SiparisKalem().GetSiparisKalem(this.siparisID);
            return true;
        }

        public int Kaydet(int adresID, int odemeTipi, string siparisNotu)
        {
            Kullanici musteri = Site.GetMusteri();
            if (musteri == null)
                return 0;

            List<Sepet> sepet = Site.GetSepet(true);
            if (sepet.Count <= 0)
                return 0;

            Adres adres = new Adres();
            if(!adres.LoadFromID(adresID))
                return 0;

            int siparisDurum = 0;
            if (odemeTipi == (int)odemeTipleri.havale)
                siparisDurum = (int)Helper.siparisDurum.odemeBekliyor;
            else if (odemeTipi == (int)odemeTipleri.kapidaOdeme)
                siparisDurum = (int)Helper.siparisDurum.hazirlaniyor;
            else if (odemeTipi == (int)odemeTipleri.krediBankaKarti)
                siparisDurum = (int)Helper.siparisDurum.odemeBekliyor;
            else if (odemeTipi == (int)odemeTipleri.magazadanTeslim)
                siparisDurum = (int)Helper.siparisDurum.hazirlaniyor;

            string siparisID = SQL.get("INSERT INTO siparisler (kaydedenKullaniciID, ad, soyad, telefon, sehir, semt, mahalle, postaKodu, adresSatir1, adresSatir2, kullaniciID, siparisDurum, odemeTipi, siparisNotu, kargoNo) VALUES " +
                "(" + musteri.kullaniciID + ", '" + adres.ad + "', '" + adres.soyad + "', '" + adres.telefon + "', '" + adres.sehir + "', '" + adres.semt + "', '" + adres.mahalle + "', '" + adres.postaKodu + "', '" + adres.adresSatir1 + "', " + 
                "'" + adres.adresSatir2 + "', " + musteri.kullaniciID + ", " + siparisDurum + ", " + odemeTipi + ", '" + siparisNotu + "', ''); SELECT SCOPE_IDENTITY();").Rows[0][0].ToString();

            foreach (Sepet sepetItem in sepet)
            {
                SQL.set("INSERT INTO siparisKalemleri (kaydedenKullaniciID, siparisID, urunID, miktar, fiyat) VALUES (" + musteri.kullaniciID + ", " + siparisID + ", " + sepetItem.urun.urunID + ", " + sepetItem.miktar + ", " + sepetItem.urun.fiyat.ToString().Replace(',', '.') + ")");
                SQL.set("UPDATE urunler SET miktar = miktar - " + sepetItem.miktar + " WHERE urunID = " + sepetItem.urun.urunID);//STOKLARINI DÜŞÜR
            }

            string siteUrl = string.Format("{0}://{1}{2}{3}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port == 80 ? string.Empty : ":" + HttpContext.Current.Request.Url.Port, HttpContext.Current.Request.ApplicationPath);
            HttpContext.Current.Session["sepet"] = null;
            Site.MailGonder("Yeni Sipariş", "<h2>Tebrikler!</h2><p>#" + siparisID + " sipariş no ile yeni bir siparişiniz var.</p><p><a href='" + siteUrl + "/Admin'>Siparişe Git</a></p>", new List<string>{ Site.siteBilgileri.eMail });
            return Convert.ToInt32(siparisID);
        }
    }
}