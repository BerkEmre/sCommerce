using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sCommerce.Helper
{
    public enum filtremeleTipleri { 
        onerilenSiralama = 0, 
        dusukFiyat = 1, 
        yuksekFiyat = 2
    }

    public enum odemeTipleri
    {
        krediBankaKarti = 0,
        havale = 1,
        kapidaOdeme = 2,
        magazadanTeslim = 3
    }

    public enum siparisDurum
    {
        odemeBekliyor = 0,
        hazirlaniyor = 1,
        odemeBasarisiz = 2,
        iptalEdildi = 3,
        kargoda = 4,
        teslimEdildi = 5
    }
}