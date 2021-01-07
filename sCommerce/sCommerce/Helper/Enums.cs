using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sCommerce.Helper
{
    public enum filtremeleTipleri { 
        onerilenSiralama = 0, 
        dusukFiyat = 1, 
        yuksekFiyat = 2,
        urunAdi = 3,
        urunAdiTersten = 4,
        urunID = 5,
        urunIDTersten = 6
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

    public enum urunEtiketleri
    {
        yok = 16,	 
        indirim = 17,
        cokSatan = 18,
        stoklarda = 19,
        yeni = 20
    }

    public enum kategoriTipleri
    {
        blog = 3,
        urun = 4
    }

    public enum urunDurumu
    {
        pasif = 9,
        aktif = 10
    }

    public enum stokBitince
    {
        satisaDevam = 11,
        satisiBitir = 12
    }

    public enum vergi
    {
        Yuzde0 = 13,
        Yuzde8 = 14,
        Yuzde18 = 15
    }
}