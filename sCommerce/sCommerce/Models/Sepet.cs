using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sCommerce.Models
{
    public class Sepet
    {
        public Urun urun;
        public int miktar;

        public Sepet(Urun urun, int miktar)
        {
            this.urun = urun;
            this.miktar = miktar;
        }
    }
}