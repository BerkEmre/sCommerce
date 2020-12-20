using System.Collections.Generic;

namespace sCommerce.Models
{
    public static class Site
    {
        public static SiteBilgileri siteBilgileri;
        public static List<Kategori> urunKategorileri;
        public static List<Kategori> blogKategorileri;

        public static void load()
        {
            siteBilgileri = new SiteBilgileri();
            siteBilgileri.load();

            urunKategorileri = new Kategori().loadKategoriler(4);
            blogKategorileri = new Kategori().loadKategoriler(3);
        }
    }
}