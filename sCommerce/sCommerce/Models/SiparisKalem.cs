using sCommerce.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace sCommerce.Models
{
    public class SiparisKalem
    {
        public int siparisKalemID;
        public int kaydedenKullaniciID;
        public DateTime kayitTarihi;
        public int guncelleyenKullaniciID;
        public DateTime guncellemeTarihi;
        public int silindi;
        public int siparisID;
        public int urunID;
        public int miktar;
        public decimal fiyat;
        public Urun urun;

        public List<SiparisKalem> GetSiparisKalem(int siparisID)
        {
            List<SiparisKalem> siparisKalems = new List<SiparisKalem>();

            DataTable dt = SQL.get("SELECT * FROM siparisKalemleri WHERE silindi = 0 AND siparisID = " + siparisID);
            foreach (DataRow dataRow in dt.Rows)
            {
                SiparisKalem sk = new SiparisKalem();
                Int32.TryParse(dataRow["siparisKalemID"].ToString(), out sk.siparisKalemID);
                Int32.TryParse(dataRow["kaydedenKullaniciID"].ToString(), out sk.kaydedenKullaniciID);
                DateTime.TryParse(dataRow["kayitTarihi"].ToString(), out sk.kayitTarihi);
                Int32.TryParse(dataRow["guncelleyenKullaniciID"].ToString(), out sk.guncelleyenKullaniciID);
                DateTime.TryParse(dataRow["guncellemeTarihi"].ToString(), out sk.guncellemeTarihi);
                Int32.TryParse(dataRow["silindi"].ToString(), out sk.silindi);
                Int32.TryParse(dataRow["siparisID"].ToString(), out sk.siparisID);
                Int32.TryParse(dataRow["urunID"].ToString(), out sk.urunID);
                Int32.TryParse(dataRow["miktar"].ToString(), out sk.miktar);
                Decimal.TryParse(dataRow["fiyat"].ToString(), out sk.fiyat);
                sk.urun = new Urun();
                sk.urun.LoadFromID(sk.urunID);
                siparisKalems.Add(sk);
            }

            return siparisKalems;
        }
    }
}