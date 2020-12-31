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

        public Sepet()
        {

        }

        public bool DegisiklikVarmi()
        {
            List<Sepet> sepet = Site.GetSepet();

            for (int i = 0; i < sepet.Count; i++)
            {
                Sepet sepetItem = sepet[i];
                sepetItem.urun.LoadFromID(sepetItem.urun.urunID);
                if (sepetItem.urun.miktar < sepetItem.miktar && sepetItem.urun.stokBitinceParametreID == 12)
                    return false;

                if (sepetItem.miktar == 0)
                    return false;
            }

            return false;
        }
    }
}