using sCommerce.Helper;
using System;
using System.Collections.Generic;
using System.Data;

namespace sCommerce.Models
{
    public class Kategori
    {
        public int kategoriID;
        public DateTime kayitTarihi;
        public int kaydedenKullaniciID;
        public DateTime guncellemeTarihi;
        public int guncelleyenKullaniciID;
        public int silindi;
        public string kategori;
        public int ustKategoriID;
        public int kategoriTipiParametreID;
        public string ikon;
        public string resim;
        public string aciklama;

        public List<Kategori> altKategori = new List<Kategori>();

        public List<Kategori> loadKategoriler(int kategoriTipiParametreID)
        {
            List<Kategori> kategoriler = new List<Kategori>();

            DataTable dt = SQL.get("SELECT * FROM kategoriler WHERE silindi = 0 AND ustKategoriID = 0 AND kategoriTipiParametreID = " + kategoriTipiParametreID);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                Kategori k = new Kategori();
                k.kategoriID = Convert.ToInt32(dr["kategoriID"]);
                k.kayitTarihi = Convert.ToDateTime(dr["kayitTarihi"]);
                k.kaydedenKullaniciID = Convert.ToInt32(dr["kaydedenKullaniciID"]);
                Int32.TryParse(dr["guncelleyenKullaniciID"].ToString(), out k.guncelleyenKullaniciID);
                DateTime.TryParse(dr["guncellemeTarihi"].ToString(), out k.guncellemeTarihi);
                k.silindi = Convert.ToInt32(dr["silindi"]);
                k.kategori = Convert.ToString(dr["kategori"]);
                k.ustKategoriID = Convert.ToInt32(dr["ustKategoriID"]);
                k.kategoriTipiParametreID = Convert.ToInt32(dr["kategoriTipiParametreID"]);
                k.ikon = Convert.ToString(dr["ikon"]);
                k.resim = Convert.ToString(dr["resim"]);
                k.aciklama = Convert.ToString(dr["aciklama"]);
                k.loadAltKategori();

                kategoriler.Add(k);
            }

            return kategoriler;
        }

        public void loadAltKategori()
        {
            List<Kategori> altKategori = new List<Kategori>();
            
            DataTable dt = SQL.get("SELECT * FROM kategoriler WHERE silindi = 0 AND ustKategoriID = " + this.kategoriID);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                Kategori k = new Kategori();
                k.kategoriID = Convert.ToInt32(dr["kategoriID"]);
                k.kayitTarihi = Convert.ToDateTime(dr["kayitTarihi"]);
                k.kaydedenKullaniciID = Convert.ToInt32(dr["kaydedenKullaniciID"]);
                Int32.TryParse(dr["guncelleyenKullaniciID"].ToString(), out k.guncelleyenKullaniciID);
                DateTime.TryParse(dr["guncellemeTarihi"].ToString(), out k.guncellemeTarihi);
                k.silindi = Convert.ToInt32(dr["silindi"]);
                k.kategori = Convert.ToString(dr["kategori"]);
                k.ustKategoriID = Convert.ToInt32(dr["ustKategoriID"]);
                k.kategoriTipiParametreID = Convert.ToInt32(dr["kategoriTipiParametreID"]);
                k.ikon = Convert.ToString(dr["ikon"]);
                k.resim = Convert.ToString(dr["resim"]);
                k.aciklama = Convert.ToString(dr["aciklama"]);
                k.loadAltKategori();

                altKategori.Add(k);
            }
            this.altKategori = altKategori;
        }
    }
}