using sCommerce.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace sCommerce.Models
{
    public class Ozellik
    {
        public int ozellikID;
        public DateTime kayitTarihi;
        public int kaydedenKullaniciID;
        public DateTime guncellemeTarihi;
        public int guncelleyenKullaniciID;
        public int silindi;
        public string ozellik;

        public Ozellik()
        {

        }

        public Ozellik(int ozellikID, DateTime kayitTarihi, int kaydedenKullaniciID, DateTime guncellemeTarihi, int guncelleyenKullaniciID, int silindi, string ozellik)
        {
            this.ozellikID = ozellikID;
            this.kayitTarihi = kayitTarihi;
            this.kaydedenKullaniciID = kaydedenKullaniciID;
            this.guncellemeTarihi = guncellemeTarihi;
            this.guncelleyenKullaniciID = guncelleyenKullaniciID;
            this.silindi = silindi;
            this.ozellik = ozellik;
        }

        public List<Ozellik> GetOzellikler()
        {
            List<Ozellik> ozelliks = new List<Ozellik>();

            DataTable dt = SQL.get("SELECT * FROM ozellikler WHERE silindi = 0");

            foreach (DataRow dataRow in dt.Rows)
            {
                Ozellik ozellik = new Ozellik();
                Int32.TryParse(dataRow["ozellikID"].ToString(), out ozellik.ozellikID);
                DateTime.TryParse(dataRow["kayitTarihi"].ToString(), out ozellik.kayitTarihi);
                Int32.TryParse(dataRow["kaydedenKullaniciID"].ToString(), out ozellik.kaydedenKullaniciID);
                DateTime.TryParse(dataRow["guncellemeTarihi"].ToString(), out ozellik.guncellemeTarihi);
                Int32.TryParse(dataRow["guncelleyenKullaniciID"].ToString(), out ozellik.guncelleyenKullaniciID);
                Int32.TryParse(dataRow["silindi"].ToString(), out ozellik.silindi);
                ozellik.ozellik = dataRow["ozellik"].ToString();

                ozelliks.Add(ozellik);
            }

            return ozelliks;
        }
    }
}