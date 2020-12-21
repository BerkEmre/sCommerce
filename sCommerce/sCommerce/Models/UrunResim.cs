using sCommerce.Helper;
using System;
using System.Collections.Generic;
using System.Data;

namespace sCommerce.Models
{
    public class UrunResim
    {
        public int urunResimID;
        public DateTime kayitTarihi;
        public int kaydedenKullaniciID;
        public DateTime guncellemeTarihi;
        public int guncelleyenKullaniciID;
        public int silindi;
        public int urunID;
        public string resim;
        public int anaResim;
        public int sira;

        public List<UrunResim> GetUrunResimleri(int urunID)
        {
            List<UrunResim> urunResims = new List<UrunResim>();

            DataTable dt = SQL.get("SELECT * FROM urunResim WHERE silindi = 0 AND urunID = " + urunID);
            
            foreach (DataRow dr in dt.Rows)
            {
                UrunResim urunResim = new UrunResim();

                Int32.TryParse(dr["urunResimID"].ToString(), out urunResim.urunResimID);
                DateTime.TryParse(dr["kayitTarihi"].ToString(), out urunResim.kayitTarihi);
                Int32.TryParse(dr["kaydedenKullaniciID"].ToString(), out urunResim.kaydedenKullaniciID);
                DateTime.TryParse(dr["guncellemeTarihi"].ToString(), out urunResim.guncellemeTarihi);
                Int32.TryParse(dr["guncelleyenKullaniciID"].ToString(), out urunResim.guncelleyenKullaniciID);
                Int32.TryParse(dr["silindi"].ToString(), out urunResim.silindi);
                Int32.TryParse(dr["urunID"].ToString(), out urunResim.urunID);
                urunResim.resim = dr["resim"].ToString();
                Int32.TryParse(dr["anaResim"].ToString(), out urunResim.anaResim);
                Int32.TryParse(dr["sira"].ToString(), out urunResim.sira);

                urunResims.Add(urunResim);
            }

            return urunResims;
        }
    }
}