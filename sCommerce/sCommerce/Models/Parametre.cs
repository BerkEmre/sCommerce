using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sCommerce.Models
{
    public class Parametre
    {
        public int parametreID;
        public string deger;

        public Parametre(int parametreID, string deger)
        {
            this.parametreID = parametreID;
            this.deger = deger;
        }
    }
}