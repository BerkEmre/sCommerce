using sCommerce.Helper;
using System;
using System.Data;

namespace sCommerce.Models
{
    public class Kullanici
    {
        public int kullaniciID;
        public DateTime kayitTarihi;
        public int kaydedenKullaniciID;
        public DateTime guncellemeTarihi;
        public int guncelleyenKullaniciID;
        public int silindi;
        public string ad;
        public string soyad;
        public string sifre;
        public string eMail;
        public int kullaniciTipiParametreID;
        public bool isValid = false;

        public bool load(int kullaniciID)
        {
            DataTable dt = SQL.get("SELECT * FROM kullanicilar WHERE kullaniciID = " + kullaniciID);
            if(dt.Rows.Count <= 0)
            {
                isValid = false;
                return false;
            }

            DataRow dr = dt.Rows[0];
            this.kullaniciID = Convert.ToInt32(dr["kullaniciID"]);
            this.kayitTarihi = Convert.ToDateTime(dr["kayitTarihi"]);
            this.kaydedenKullaniciID = Convert.ToInt32(dr["kaydedenKullaniciID"]);
            DateTime.TryParse(dr["guncellemeTarihi"].ToString(), out this.guncellemeTarihi);
            Int32.TryParse(dr["guncelleyenKullaniciID"].ToString(), out this.guncelleyenKullaniciID);
            this.silindi = Convert.ToInt32(dr["silindi"]);
            this.ad = Convert.ToString(dr["ad"]);
            this.soyad = Convert.ToString(dr["soyad"]);
            this.sifre = Convert.ToString(dr["sifre"]);
            this.eMail = Convert.ToString(dr["eMail"]);
            this.kullaniciTipiParametreID = Convert.ToInt32(dr["kullaniciTipiParametreID"]);
            this.isValid = true;

            return true;
        }

        public string adSoyad()
        {
            return ad + " " + soyad;
        }
    }
}