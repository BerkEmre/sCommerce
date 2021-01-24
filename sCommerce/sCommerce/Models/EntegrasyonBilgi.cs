using sCommerce.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace sCommerce.Models
{
    public class EntegrasyonBilgi
    {
        public int entegrasyonBilgiID;
        public DateTime kayitTarihi;
        public int kaydedenKullaniciID;
        public DateTime guncellemeTarihi;
        public int guncelleyenKullaniciID;
        public int silindi;
        public int urunID;
        public long n11UrunID;
        public long n11KategoriID;
        public string n11KategoriAdi;
        public decimal n11FiyatYuzde;
        public decimal n11FiyatArti;
        public string n11TemplateName;

        public bool N11KaydetGuncelle(int urunID, int kullaniciID, long n11UrunID, long n11KategoriID, string n11KategoriAdi, decimal n11FiyatYuzde, decimal n11FiyatArti, string n11TemplateName)
        {
            DataTable dt = SQL.get("SELECT * FROM entegrasyonBilgileri WHERE silindi = 0 AND urunID = " + urunID);
            if(dt.Rows.Count > 0)
            {
                SQL.set(
                    "UPDATE " +
                    "   entegrasyonBilgileri " +
                    "SET " +
                    "   guncelleyenKullaniciID = " + kullaniciID + ", " +
                    "   guncellemeTarihi = GETDATE(), " +
                    "   n11KategoriID = '" + n11KategoriID + "', " +
                    "   n11KategoriAdi = '" + n11KategoriAdi + "', " +
                    "   n11FiyatYuzde = " + n11FiyatYuzde.ToString().Replace(',', '.') + ", " +
                    "   n11FiyatArti = " + n11FiyatArti.ToString().Replace(',', '.') + ", " +
                    "   n11TemplateName = '" + n11TemplateName + "' " +
                    "WHERE " +
                    "   silindi = 0 " +
                    "   AND urunID = " + urunID);
            }
            else
            {
                SQL.set(
                    "INSERT INTO entegrasyonBilgileri (kaydedenKullaniciID, urunID, n11urunID, n11KategoriID, n11KategoriAdi, n11FiyatYuzde, n11FiyatArti, n11TemplateName) " +
                    "VALUES (" + kullaniciID + ", " + urunID + ", '', '" + n11KategoriID + "', '" + n11KategoriAdi + "', " + n11FiyatYuzde.ToString().Replace(',', '.') + ", " + n11FiyatArti.ToString().Replace(',', '.') + ", " +
                    "'" + n11TemplateName + "')");
            }
            return true;
        }

        public bool N11UrunIDGuncelle(int urunID, long n11UrunID)
        {
            SQL.set(
                    "UPDATE " +
                    "   entegrasyonBilgileri " +
                    "SET " +
                    "   guncellemeTarihi = GETDATE(), " +
                    "   n11UrunID = " + n11UrunID + " " +
                    "WHERE " +
                    "   silindi = 0 " +
                    "   AND urunID = " + urunID);

            return true;
        }

        public decimal GetN11Fiyat(decimal fiyat)
        {
            decimal yeniFiyat = 0;
            fiyat = fiyat + n11FiyatArti;
            yeniFiyat = (100 * fiyat) / (100 - n11FiyatYuzde);
            yeniFiyat = decimal.Round(yeniFiyat, 2);
            return yeniFiyat;
        }
    }
}