using sCommerce.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace sCommerce.Models
{
    public class Marka
    {
        public int markaID;
        public DateTime kayitTarihi;
        public int kaydedenKullaniciID;
        public DateTime guncellemeTarihi;
        public int guncelleyenKullaniciID;
        public int silindi;
        public string marka;

        public Marka(int markaID, DateTime kayitTarihi, int kaydedenKullaniciID, DateTime guncellemeTarihi, int guncelleyenKullaniciID, int silindi, string marka)
        {
            this.markaID = markaID;
            this.kayitTarihi = kayitTarihi;
            this.kaydedenKullaniciID = kaydedenKullaniciID;
            this.guncellemeTarihi = guncellemeTarihi;
            this.guncelleyenKullaniciID = guncelleyenKullaniciID;
            this.silindi = silindi;
            this.marka = marka;
        }

        public Marka()
        {

        }

        public List<Marka> GetMarkalar()
        {
            List<Marka> markas = new List<Marka>();

            DataTable dt = SQL.get("SELECT * FROM markalar WHERE silindi = 0");
            foreach (DataRow dataRow in dt.Rows)
            {
                Marka m = new Marka();
                Int32.TryParse(dataRow["markaID"].ToString(), out m.markaID);
                DateTime.TryParse(dataRow["kayitTarihi"].ToString(), out m.kayitTarihi);
                Int32.TryParse(dataRow["kaydedenKullaniciID"].ToString(), out m.kaydedenKullaniciID);
                DateTime.TryParse(dataRow["guncellemeTarihi"].ToString(), out m.guncellemeTarihi);
                Int32.TryParse(dataRow["guncelleyenKullaniciID"].ToString(), out m.guncelleyenKullaniciID);
                Int32.TryParse(dataRow["silindi"].ToString(), out m.silindi);
                m.marka = dataRow["marka"].ToString();
                markas.Add(m);
            }

            return markas;
        }
    }
}