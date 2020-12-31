using sCommerce.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace sCommerce.Models
{
    public class ModelGrubu
    {
        public int modelGrubuID;
        public DateTime kayitTarihi;
        public int kaydedenKullaniciID;
        public DateTime guncellemeTarihi;
        public int guncelleyenKullaniciID;
        public int silindi;
        public string modelGrubu;

        public ModelGrubu(int modelGrubuID, DateTime kayitTarihi, int kaydedenKullaniciID, DateTime guncellemeTarihi, int guncelleyenKullaniciID, int silindi, string modelGrubu)
        {
            this.modelGrubuID = modelGrubuID;
            this.kayitTarihi = kayitTarihi;
            this.kaydedenKullaniciID = kaydedenKullaniciID;
            this.guncellemeTarihi = guncellemeTarihi;
            this.guncelleyenKullaniciID = guncelleyenKullaniciID;
            this.silindi = silindi;
            this.modelGrubu = modelGrubu;
        }

        public ModelGrubu()
        {

        }

        public ModelGrubu Load(int modelGrubuID)
        {
            ModelGrubu modelGrubu = new ModelGrubu();

            DataTable dt = SQL.get("SELECT * FROM modelGrubu WHERE silindi = 0 AND modelGrubuID = " + modelGrubuID);

            if (dt.Rows.Count == 0)
                return modelGrubu;
            DataRow dr = dt.Rows[0];
            Int32.TryParse(dr["modelGrubuID"].ToString(), out modelGrubu.modelGrubuID);
            DateTime.TryParse(dr["kayitTarihi"].ToString(), out modelGrubu.kayitTarihi);
            Int32.TryParse(dr["kaydedenKullaniciID"].ToString(), out modelGrubu.kaydedenKullaniciID);
            DateTime.TryParse(dr["guncellemeTarihi"].ToString(), out modelGrubu.guncellemeTarihi);
            Int32.TryParse(dr["guncelleyenKullaniciID"].ToString(), out modelGrubu.guncelleyenKullaniciID);
            Int32.TryParse(dr["silindi"].ToString(), out modelGrubu.silindi);
            modelGrubu.modelGrubu = dr["modelGrubu"].ToString();

            return modelGrubu;
        }
    }
}