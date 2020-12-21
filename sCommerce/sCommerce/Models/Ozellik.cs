using System;
using System.Collections.Generic;
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
    }
}