using sCommerce.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace sCommerce.Models
{
    public class Urun
    {
        public int urunID;
        public DateTime kayitTarihi;
        public int kaydedenKullaniciID;
        public DateTime guncellemeTarihi;
        public int guncelleyenKullaniciID;
        public int silindi;
        public string urunAdi;
        public string urunAciklamasi;
        public string seoAciklama;
        public string seoKeywords;
        public int urunEtiketiParametreID;
        public string barkod;
        public string stokKodu;
        public string depoLokasyonu;
        public decimal eskiFiyat;
        public decimal fiyat;
        public int vergiParametreID;
        public int vergiDahilSatis;
        public int miktar;
        public int minimumMiktar;
        public int stokBitinceParametreID;
        public decimal agirlik;
        public int kargoSuresi;
        public int urunDurumuParametreID;
        public int oneCikanlar;
        public int modelGrubuID;
        public Parametre urunEtiketi;
        public Parametre vergi;
        public Parametre stokBitince;
        public Parametre urunDurumu;
        public ModelGrubu modelGrubu;
        public List<Kategori> urunKategorileri;
        public List<UrunResim> urunResimleri;
        public List<UrunOzellik> urunOzellikleri;

        public List<Urun> GetUrunFromUrunEtiketi(int urunEtiketiParametreID)
        {
            List<Urun> u = new List<Urun>();

            DataTable dt = SQL.get(
                "SELECT " +
                "    u.*, " +
                "    urunEtiketi = pUEtiket.deger, " +
                "    vergi = pVergi.deger, " +
                "    stokBitince = pSBitince.deger, " +
                "    urunDurumu = pUDurumu.deger, " +
                "    modelGrubu = mg.modelGrubu " +
                "FROM " +
                "    urunler u " +
                "    LEFT OUTER JOIN parametreler pUEtiket ON pUEtiket.parametreID = u.urunEtiketiParametreID " +
                "    LEFT OUTER JOIN parametreler pVergi ON pVergi.parametreID = u.vergiParametreID " +
                "    LEFT OUTER JOIN parametreler pSBitince ON pSBitince.parametreID = u.stokBitinceParametreID " +
                "    LEFT OUTER JOIN parametreler pUDurumu ON pUDurumu.parametreID = u.urunDurumuParametreID " +
                "    LEFT OUTER JOIN modelGrubu mg ON mg.modelGrubuID = u.modelGrubuID " +
                "WHERE " +
                "    u.silindi = 0 " +
                "    AND u.oneCikanlar = 1 " +
                "    AND u.urunEtiketiParametreID = " + urunEtiketiParametreID);

            foreach (DataRow dataRow in dt.Rows)
            {
                Urun urun = new Urun();
                Int32.TryParse(dataRow["urunID"].ToString(), out urun.urunID);
                DateTime.TryParse(dataRow["kayitTarihi"].ToString(), out urun.kayitTarihi);
                Int32.TryParse(dataRow["kaydedenKullaniciID"].ToString(), out urun.kaydedenKullaniciID);
                DateTime.TryParse(dataRow["guncellemeTarihi"].ToString(), out urun.guncellemeTarihi);
                Int32.TryParse(dataRow["guncelleyenKullaniciID"].ToString(), out urun.guncelleyenKullaniciID);
                Int32.TryParse(dataRow["silindi"].ToString(), out urun.silindi);
                urun.urunAdi = dataRow["urunAdi"].ToString();
                urun.urunAciklamasi = dataRow["urunAciklamasi"].ToString();
                urun.seoAciklama = dataRow["seoAciklama"].ToString();
                urun.seoKeywords = dataRow["urunEtiketiParametreID"].ToString();
                Int32.TryParse(dataRow["urunEtiketiParametreID"].ToString(), out urun.urunEtiketiParametreID);
                urun.barkod = dataRow["barkod"].ToString();
                urun.stokKodu = dataRow["stokKodu"].ToString();
                urun.depoLokasyonu = dataRow["depoLokasyonu"].ToString();
                Decimal.TryParse(dataRow["eskiFiyat"].ToString(), out urun.eskiFiyat);
                Decimal.TryParse(dataRow["fiyat"].ToString(), out urun.fiyat);
                Int32.TryParse(dataRow["vergiParametreID"].ToString(), out urun.vergiParametreID);
                Int32.TryParse(dataRow["vergiDahilSatis"].ToString(), out urun.vergiDahilSatis);
                Int32.TryParse(dataRow["miktar"].ToString(), out urun.miktar);
                Int32.TryParse(dataRow["minimumMiktar"].ToString(), out urun.minimumMiktar);
                Int32.TryParse(dataRow["stokBitinceParametreID"].ToString(), out urun.stokBitinceParametreID);
                Decimal.TryParse(dataRow["agirlik"].ToString(), out urun.agirlik);
                Int32.TryParse(dataRow["kargoSuresi"].ToString(), out urun.kargoSuresi);
                Int32.TryParse(dataRow["urunDurumuParametreID"].ToString(), out urun.urunDurumuParametreID);
                Int32.TryParse(dataRow["oneCikanlar"].ToString(), out urun.oneCikanlar);
                Int32.TryParse(dataRow["modelGrubuID"].ToString(), out urun.modelGrubuID);
                urun.urunEtiketi = new Parametre(urun.urunEtiketiParametreID, dataRow["urunEtiketi"].ToString());
                urun.vergi = new Parametre(urun.urunEtiketiParametreID, dataRow["vergi"].ToString());
                urun.stokBitince = new Parametre(urun.urunEtiketiParametreID, dataRow["stokBitince"].ToString());
                urun.urunDurumu = new Parametre(urun.urunEtiketiParametreID, dataRow["urunDurumu"].ToString());
                urun.modelGrubu = new ModelGrubu(urun.modelGrubuID, DateTime.Now, 0, DateTime.Now, 0, 0, dataRow["modelGrubu"].ToString());
                //urun.urunKategorileri = new Kategori().GetUrunKategorileri(urun.urunID);
                urun.urunResimleri = new UrunResim().GetUrunResimleri(urun.urunID);
                //urun.urunOzellikleri = new UrunOzellik().GetUrunOzellik(urun.urunID);

                u.Add(urun);
            }
            return u;
        }
    }
}