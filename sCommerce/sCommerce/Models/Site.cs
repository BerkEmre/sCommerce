using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;

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

        public static List<Favori> GetFavori(bool urunLoad = false)
        {
            List<Favori> favoriler = new List<Favori>();

            Kullanici musteri = GetMusteri();
            if(musteri != null)
            {
                favoriler = new Favori().GetKullaniciFavori(musteri.kullaniciID, urunLoad);
            }

            return favoriler;
        }

        public static bool SetFavori(Urun urun)
        {
            bool ret;
            Kullanici musteri = GetMusteri();

            if (musteri != null)
            {
                List<Favori> favoriler = GetFavori();

                int cnt = favoriler.Where(x => x.urunID == urun.urunID).Count();

                if (cnt == 0)
                {
                    new Favori().Kaydet(urun.urunID, musteri.kullaniciID);
                    ret = true;
                }
                else
                {
                    new Favori().Sil(urun.urunID, musteri.kullaniciID);
                    ret = false;
                }
            }
            else
                ret = false;
            

            return ret;
        }

        public static List<Sepet> GetSepet(bool stok = false)
        {
            List<Sepet> sepet = new List<Sepet>();
            Urun urun = new Urun();
            if (HttpContext.Current.Session["sepet"] != null)
            {
                sepet = (List<Sepet>)HttpContext.Current.Session["sepet"];
            }

            if (stok)
            {
                for (int i = 0; i < sepet.Count; i++)
                {
                    Sepet sepetItem = sepet[i];
                    sepetItem.urun.LoadFromID(sepetItem.urun.urunID);
                    if (sepetItem.urun.miktar < sepetItem.miktar && sepetItem.urun.stokBitinceParametreID == 12)
                        sepetItem.miktar = sepetItem.urun.miktar;

                    if(sepetItem.miktar == 0)
                        sepet.Remove(sepetItem);
                }
            }

            HttpContext.Current.Session["sepet"] = sepet;
            return sepet;
        }

        public static bool SetSepet(Urun urun, int miktar, bool ekleCikar)
        {
            List<Sepet> sepet = GetSepet();
            bool ret = ekleCikar;
            int yeniMiktar = 0;

            int cnt = sepet.Where(x => x.urun.urunID == urun.urunID).Count();
            if (ekleCikar)
            {
                if(cnt == 0)
                {
                    yeniMiktar = miktar > urun.miktar && urun.stokBitinceParametreID == 12 ? urun.miktar : miktar;
                    sepet.Add(new Sepet(urun, yeniMiktar));
                }
                else
                {
                    Sepet s = sepet.First(x => x.urun.urunID == urun.urunID);
                    yeniMiktar = s.miktar + miktar;
                    yeniMiktar = yeniMiktar > urun.miktar && urun.stokBitinceParametreID == 12 ? urun.miktar : yeniMiktar;
                    s.miktar = yeniMiktar;
                }
            }
            else
            {
                if (cnt > 0)
                {
                    Sepet s = sepet.First(x => x.urun.urunID == urun.urunID);
                    if (s.miktar <= miktar)
                        sepet.Remove(s);
                    else
                        s.miktar -= miktar;
                }
            }

            HttpContext.Current.Session["sepet"] = sepet;

            return ret;
        }

        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
    
        public static bool MailGonder(string konu, string icerik, List<string> toMail)
        {
            try
            {
                SmtpClient SmtpServer = new SmtpClient();
                MailMessage msj = new MailMessage();
                msj.From = new MailAddress(ConfigurationManager.AppSettings["mail_adresi"], siteBilgileri.siteAdi, System.Text.Encoding.UTF8);
                foreach (string to in toMail)
                {
                    msj.To.Add(to);
                }
                msj.IsBodyHtml = true;
                msj.Subject = konu;
                msj.Body = icerik; 
                SmtpServer.Send(msj); 
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        static public void CookieCreate(string cookiename, string value)
        {
            HttpContext.Current.Response.Cookies[cookiename].Value = value;
            HttpContext.Current.Response.Cookies[cookiename].Expires = DateTime.Now.AddDays(10);
        }

        static public HttpCookie CookieGet(string cookiename)
        {
            if (HttpContext.Current.Request.Cookies[cookiename] != null)
            {
                HttpContext.Current.Request.Cookies[cookiename].Expires = DateTime.Now.AddDays(10);
                return HttpContext.Current.Request.Cookies[cookiename];
            }
            else
                return null;
        }

        static public void CookieDelete(string cookiename)
        {
            HttpContext.Current.Response.Cookies[cookiename].Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Request.Cookies.Remove(cookiename);
        }
    
        static public Kullanici GetMusteri()
        {
            Kullanici musteri = null;

            if (HttpContext.Current.Session["musteri"] != null)
            {
                musteri = (Kullanici)HttpContext.Current.Session["musteri"];
            }
            else
            {
                HttpCookie eMail = CookieGet("eMail");
                HttpCookie sifre = CookieGet("sifre");

                if(eMail != null)
                {
                    musteri = new Kullanici();
                    if (musteri.GirisKontrol(eMail.Value, sifre.Value, false).Length > 1)
                        musteri = null;
                }
            }

            return musteri;
        }
    }
}