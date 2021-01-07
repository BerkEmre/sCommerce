using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using sCommerce.Helper;
using sCommerce.Models;

namespace sCommerce.Controllers
{
    public class FacebookStoreController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {
            string siteUrl = string.Format("{0}://{1}{2}{3}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port == 80 ? string.Empty : ":" + HttpContext.Current.Request.Url.Port, HttpContext.Current.Request.ApplicationPath);

            string XML = "";

            UrunFilter urunFilter = new UrunFilter("", new List<int>(), new List<int>(), new List<int>(), new List<int>(), filtremeleTipleri.urunID, false, 1, 10000);
            List<Urun> urunler = urunFilter.GetUruns();

            XML += "<?xml version=\"1.0\"?>";
            XML += "<rss xmlns:g=\"http://base.google.com/ns/1.0\" version=\"2.0\">";
            XML += "<channel>";
            XML += "<title>Ürünler</title>";
            XML += "<link href=\"" + siteUrl + "\" rel=\"self\"/>";
            foreach (Urun urun in urunler)
            {
                XML += "<item>";
                XML += "<g:id>" + urun.urunID + "</g:id>";
                XML += "<g:title>" + urun.urunAdi + "</g:title>";
                XML += "<g:description>" + urun.marka.marka + "</g:description>";
                XML += "<g:link>" + siteUrl + "/Home/Product/" + urun.urunID + "</g:link>";
                XML += "<g:image_link>" + siteUrl + "/Content/icerik/urun/kucuk/" + urun.urunResimleri[0].resim + "</g:image_link>";
                XML += "<g:brand>" + urun.marka.marka + "</g:brand>";
                XML += "<g:condition>new</g:condition>";
                XML += "<g:availability>" + (urun.miktar > 0 || urun.stokBitinceParametreID == (int)stokBitince.satisaDevam ? "in stock" : "out of stock") + "</g:availability>";
                XML += "<g:price>" + urun.fiyat.ToString().Replace(',', '.') + " TRY</g:price>";
                XML += "</item>";
            }
            XML += "</channel>";
            XML += "</rss>";

            return new HttpResponseMessage()
            {
                Content = new StringContent(XML, Encoding.UTF8, "application/xml")
            };
        }
    }
}
