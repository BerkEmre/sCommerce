using sCommerce.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace sCommerce.Models
{
    public class Adres
    {
        public int adresID;
        public DateTime kayitTarihi;
        public int kaydedenKullaniciID;
        public DateTime guncellemeTarihi;
        public int guncelleyenKullaniciID;
        public int silindi;
        public int kullaniciID;
        public string adres;
        public string ad;
        public string soyad;
        public string telefon;
        public string sehir;
        public string semt;
        public string mahalle;
        public string adresSatir1;
        public string adresSatir2;
        public string postaKodu;

        public Adres()
        {

        }

        public bool AdresEkleGuncelle(int adresID, string adres, string ad, string soyad, string telefon, string sehir, string semt, string mahalle, string adresSatir1, string adresSatir2, string postaKodu, int kullaniciID)
        {
            if (adresID == 0)
                SQL.set("INSERT INTO adresler (kaydedenKullaniciID, adres, ad, soyad, telefon, sehir, semt, mahalle, adresSatir1, adresSatir2, postaKodu, kullaniciID) VALUES " +
                    "(" + kullaniciID + ", '" + adres + "', '" + ad + "', '" + soyad + "', '" + telefon + "', '" + sehir + "', '" + semt + "', '" + mahalle + "', '" + adresSatir1 + "', '" + adresSatir2 + "', '" + postaKodu + "', " + kullaniciID + ")");
            else
                SQL.set("UPDATE adresler SET guncelleyenKullaniciID = " + kullaniciID + ", guncellemeTarihi = GETDATE(), adres = '" + adres + "', ad = '" + ad + "', soyad = '" + soyad + "', telefon = '" + telefon + "', sehir = '" + sehir + "', semt = '" + semt + "', mahalle = '" + mahalle + "', adresSatir1 = '" + adresSatir1 + "', adresSatir2 = '" + adresSatir2 + "', postaKodu = '" + postaKodu + "' WHERE adresID = " + adresID);
            return true;
        }

        public List<Adres> GetKullaniciAdres(int kullaniciID)
        {
            List<Adres> adress = new List<Adres>();

            DataTable dt = SQL.get("SELECT * FROM adresler WHERE silindi = 0 AND kullaniciID = " + kullaniciID);
            foreach (DataRow dataRow in dt.Rows)
            {
                Adres adres = new Adres();
                Int32.TryParse(dataRow["adresID"].ToString(), out adres.adresID);
                DateTime.TryParse(dataRow["kayitTarihi"].ToString(), out adres.kayitTarihi);
                Int32.TryParse(dataRow["kaydedenKullaniciID"].ToString(), out adres.kaydedenKullaniciID);
                DateTime.TryParse(dataRow["guncellemeTarihi"].ToString(), out adres.guncellemeTarihi);
                Int32.TryParse(dataRow["guncelleyenKullaniciID"].ToString(), out adres.guncelleyenKullaniciID);
                Int32.TryParse(dataRow["silindi"].ToString(), out adres.silindi);
                Int32.TryParse(dataRow["kullaniciID"].ToString(), out adres.kullaniciID);
                adres.adres = dataRow["adres"].ToString();
                adres.ad = dataRow["ad"].ToString();
                adres.soyad = dataRow["soyad"].ToString();
                adres.telefon = dataRow["telefon"].ToString();
                adres.sehir = dataRow["sehir"].ToString();
                adres.semt = dataRow["semt"].ToString();
                adres.mahalle = dataRow["mahalle"].ToString();
                adres.adresSatir1 = dataRow["adresSatir1"].ToString();
                adres.adresSatir2 = dataRow["adresSatir2"].ToString();
                adres.postaKodu = dataRow["postaKodu"].ToString();
                adress.Add(adres);
            }
            return adress;
        }

        public bool LoadFromID(int adresID)
        {
            DataTable dt = SQL.get("SELECT * FROM adresler WHERE silindi = 0 AND adresID = " + adresID);

            if (dt.Rows.Count <= 0)
                return false;

            DataRow dataRow = dt.Rows[0];

            Int32.TryParse(dataRow["adresID"].ToString(), out this.adresID);
            DateTime.TryParse(dataRow["kayitTarihi"].ToString(), out this.kayitTarihi);
            Int32.TryParse(dataRow["kaydedenKullaniciID"].ToString(), out this.kaydedenKullaniciID);
            DateTime.TryParse(dataRow["guncellemeTarihi"].ToString(), out this.guncellemeTarihi);
            Int32.TryParse(dataRow["guncelleyenKullaniciID"].ToString(), out this.guncelleyenKullaniciID);
            Int32.TryParse(dataRow["silindi"].ToString(), out this.silindi);
            Int32.TryParse(dataRow["kullaniciID"].ToString(), out this.kullaniciID);
            this.adres = dataRow["adres"].ToString();
            this.ad = dataRow["ad"].ToString();
            this.soyad = dataRow["soyad"].ToString();
            this.telefon = dataRow["telefon"].ToString();
            this.sehir = dataRow["sehir"].ToString();
            this.semt = dataRow["semt"].ToString();
            this.mahalle = dataRow["mahalle"].ToString();
            this.adresSatir1 = dataRow["adresSatir1"].ToString();
            this.adresSatir2 = dataRow["adresSatir2"].ToString();
            this.postaKodu = dataRow["postaKodu"].ToString();

            return true;
        }
    }
}