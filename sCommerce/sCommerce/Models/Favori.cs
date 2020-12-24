using sCommerce.Helper;
using System;
using System.Collections.Generic;
using System.Data;

namespace sCommerce.Models
{
    public class Favori
    {
        public int favoriID;
        public int silindi;
        public int urunID;
        public int kullaniciID;
        public Urun urun;
        public Kullanici kullanici;

        public Favori()
        {

        }

        public Favori(int favoriID, int silindi, int urunID, int kullaniciID)
        {
            this.favoriID = favoriID;
            this.silindi = silindi;
            this.urunID = urunID;
            this.kullaniciID = kullaniciID;
        }

        public List<Favori> GetKullaniciFavori(int kullaniciID, bool urunLoad)
        {
            List<Favori> favoriler = new List<Favori>();

            DataTable dt = SQL.get("SELECT * FROM favoriler WHERE silindi = 0 AND kullaniciID = " + kullaniciID);
            foreach (DataRow dataRow in dt.Rows)
            {
                Favori f = new Favori();
                Int32.TryParse(dataRow["favoriID"].ToString(), out f.favoriID);
                Int32.TryParse(dataRow["silindi"].ToString(), out f.silindi);
                Int32.TryParse(dataRow["urunID"].ToString(), out f.urunID);
                Int32.TryParse(dataRow["kullaniciID"].ToString(), out f.kullaniciID);
                if (urunLoad)
                {
                    Urun u = new Urun();
                    u.LoadFromID(f.urunID);
                    f.urun = u;
                }

                favoriler.Add(f);
            }

            return favoriler;
        }

        public bool Kaydet(int urunID, int kullaniciID)
        {
            SQL.set("INSERT INTO favoriler (silindi, urunID, kullaniciID) VALUES (0, " + urunID + ", " + kullaniciID + ")");

            return true;
        }

        public bool Sil(int urunID, int kullaniciID)
        {
            SQL.set("UPDATE favoriler SET silindi = 1 WHERE silindi = 0 AND urunID = " + urunID + " AND kullaniciID = " + kullaniciID);

            return true;
        }
    }
}