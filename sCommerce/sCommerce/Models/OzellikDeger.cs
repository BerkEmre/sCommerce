using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sCommerce.Models
{
    public class OzellikDeger
    {
        public int ozellikDegerID;
        public DateTime kayitTarihi;
        public int kaydedenKullaniciID;
        public DateTime guncellemeTarihi;
        public int guncelleyenKullaniciID;
        public int silindi;
        public string deger;
        public int ozellikID;
        public Ozellik ozellik;

        public OzellikDeger(int ozellikDegerID,DateTime kayitTarihi, int kaydedenKullaniciID, DateTime guncellemeTarihi, int guncelleyenKullaniciID, int silindi, string deger, int ozellikID, Ozellik ozellik)
        {
            this.ozellikDegerID = ozellikDegerID;
            this.kayitTarihi = kayitTarihi;
            this.kaydedenKullaniciID = kaydedenKullaniciID;
            this.guncellemeTarihi = guncellemeTarihi;
            this.guncelleyenKullaniciID = guncelleyenKullaniciID;
            this.silindi = silindi;
            this.deger = deger;
            this.ozellikID = ozellikID;
            this.ozellik = ozellik;
    }
    }
}