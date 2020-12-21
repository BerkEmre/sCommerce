using System;
using System.Collections.Generic;
using System.Data;

namespace sCommerce.Models
{
    public class UrunOzellik
    {
        public int urunOzellikID;
        public DateTime kayitTarihi;
        public int kaydedenKullaniciID;
        public DateTime guncellemeTarihi;
        public int guncelleyenKullaniciID;
        public int silindi;
        public int urunID;
        public int degerID;
        public OzellikDeger ozellikDeger;

        public List<UrunOzellik> GetUrunOzellik(int urunID)
        {
            List<UrunOzellik> urunOzelliks = new List<UrunOzellik>();

            int ozellikID;
            int ozellikDegerID;

            DataTable dt = Helper.SQL.get("SELECT uo.*, od.deger, od.ozellikID, o.ozellik FROM urunOzellik uo INNER JOIN ozellikDegerleri od ON od.ozellikDegerID = uo.degerID INNER JOIN ozellikler o ON o.ozellikID = od.ozellikID WHERE uo.silindi = 0 AND uo.urunID = " + urunID);
            foreach (DataRow dr in dt.Rows)
            {
                UrunOzellik urunOzellik = new UrunOzellik();

                Int32.TryParse(dr["urunOzellikID"].ToString(), out urunOzellik.urunOzellikID);
                DateTime.TryParse(dr["kayitTarihi"].ToString(), out urunOzellik.kayitTarihi);
                Int32.TryParse(dr["kaydedenKullaniciID"].ToString(), out urunOzellik.kaydedenKullaniciID);
                DateTime.TryParse(dr["guncellemeTarihi"].ToString(), out urunOzellik.guncellemeTarihi);
                Int32.TryParse(dr["guncelleyenKullaniciID"].ToString(), out urunOzellik.guncelleyenKullaniciID);
                Int32.TryParse(dr["silindi"].ToString(), out urunOzellik.silindi);
                Int32.TryParse(dr["urunID"].ToString(), out urunOzellik.urunID);
                Int32.TryParse(dr["degerID"].ToString(), out urunOzellik.degerID);

                Int32.TryParse(dr["ozellikID"].ToString(), out ozellikID);
                Ozellik o = new Ozellik(ozellikID, DateTime.Now, 0, DateTime.Now, 0, 0, dr["ozellik"].ToString());

                Int32.TryParse(dr["degerID"].ToString(), out ozellikDegerID);
                urunOzellik.ozellikDeger = new OzellikDeger(ozellikDegerID, DateTime.Now, 0, DateTime.Now, 0, 0, dr["deger"].ToString(), ozellikID, o);

                urunOzelliks.Add(urunOzellik);
            }

            return urunOzelliks;
        }
    }
}