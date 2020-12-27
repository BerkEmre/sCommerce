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

        public Kategori()
        {

        }

        public Kategori(int kategoriID, string kategori)
        {
            this.kategoriID = kategoriID;
            this.kategori = kategori;
        }

        public bool LoadKategori(int kategoriID)
        {
            DataTable dt = SQL.get("SELECT * FROM kategoriler WHERE silindi = 0 AND kategoriID = " + kategoriID);
            if (dt.Rows.Count <= 0)
                return false;

            DataRow dr = dt.Rows[0];

            this.kategoriID = Convert.ToInt32(dr["kategoriID"]);
            this.kayitTarihi = Convert.ToDateTime(dr["kayitTarihi"]);
            this.kaydedenKullaniciID = Convert.ToInt32(dr["kaydedenKullaniciID"]);
            Int32.TryParse(dr["guncelleyenKullaniciID"].ToString(), out this.guncelleyenKullaniciID);
            DateTime.TryParse(dr["guncellemeTarihi"].ToString(), out this.guncellemeTarihi);
            this.silindi = Convert.ToInt32(dr["silindi"]);
            this.kategori = Convert.ToString(dr["kategori"]);
            this.ustKategoriID = Convert.ToInt32(dr["ustKategoriID"]);
            this.kategoriTipiParametreID = Convert.ToInt32(dr["kategoriTipiParametreID"]);
            this.ikon = Convert.ToString(dr["ikon"]);
            this.resim = Convert.ToString(dr["resim"]);
            this.aciklama = Convert.ToString(dr["aciklama"]);
            this.loadAltKategori();

            return true;
        }

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

        public List<Kategori> GetUrunKategorileri(int urunID)
        {
            List<Kategori> kategoris = new List<Kategori>();

            DataTable dt = SQL.get("SELECT k.* FROM urunKategori uk INNER JOIN kategoriler k ON k.kategoriID = uk.kategoriID WHERE uk.silindi = 0 AND uk.urunID = " + urunID);

            foreach (DataRow dr in dt.Rows)
            {
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

                kategoris.Add(k);
            }

            return kategoris;
        }
    }
}