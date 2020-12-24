using Newtonsoft.Json;
using sCommerce.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web.Mvc;

namespace sCommerce.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ComingSoon()
        {
            return View();
        }

        public ActionResult Page404()
        {
            return View();
        }

        public ActionResult Page(int id = 0)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Customer()
        {
            return View();
        }

        public ActionResult Wishlist()
        {
            return View();
        }

        public ActionResult RePass(string a, string b)
        {
            Kullanici musteri = new Kullanici().SifirlamaCoz(a, b);
            ViewBag.musteri = musteri;
            ViewBag.a = a;
            ViewBag.b = b;
            return View();
        }

        public ActionResult ForgotPass()
        {
            if (Site.GetMusteri() != null)
                return RedirectToAction("Index");

            return View();
        }

        public ActionResult Login()
        {
            if (Site.GetMusteri() != null)
                return RedirectToAction("Index");
            return View();
        }

        public ActionResult SifreSifirla(string eMail, string sifre, string a, string b)
        {
            Kullanici musteri = new Kullanici().SifirlamaCoz(a, b);
            if(musteri == null)
                return RedirectToAction("Index");
            musteri.SifreSifirla(eMail, sifre);
            return RedirectToAction("Login", new { hata = "Şifreniz sıfırlanmıştır, yeni şifreniz ile giriş yapabilirsiniz." });
        }

        public ActionResult CikisYap()
        {
            Session["musteri"] = null;
            Site.CookieDelete("eMail");
            Site.CookieDelete("sifre");

            return RedirectToAction("Index");
        }

        public ActionResult GirisYap(string eMail, string sifre)
        {
            Kullanici musteri = new Kullanici();
            string hata = musteri.GirisKontrol(eMail, sifre);

            if (hata.Length > 1)
                return RedirectToAction("Login", new { hata = hata });

            Session["musteri"] = musteri;
            Site.CookieCreate("eMail", musteri.eMail);
            Site.CookieCreate("sifre", musteri.sifre);

            return RedirectToAction("Index");
        }

        public ActionResult UyeOl(string ad, string soyad, string eMail, string sifre)
        {
            if (ad.Length <= 0)
                return RedirectToAction("Login", new { hata = "Lütfen adınızı girin." });
            if (soyad.Length <= 0)
                return RedirectToAction("Login", new { hata = "Lütfen soyadınızı girin." });
            if (eMail.Length <= 0)
                return RedirectToAction("Login", new { hata = "Lütfen mail adresinizi girin." });
            if (sifre.Length <= 5)
                return RedirectToAction("Login", new { hata = "Lütfen şifrenizi girin." });

            Kullanici musteri = new Kullanici();
            string hata = musteri.Kaydet(ad, soyad, sifre, eMail);

            if (hata.Length > 1)
                return RedirectToAction("Login", new { hata = hata });

            Session["musteri"] = musteri;
            Site.CookieCreate("eMail", musteri.eMail);
            Site.CookieCreate("sifre", musteri.sifre);

            return RedirectToAction("Customer");
        }

        public ActionResult SifremiUnuttum(string eMail)
        {
            if(eMail.Length <= 0)
                return RedirectToAction("ForgotPass", new { hata = "Lütfen mail adresinizi girin." });

            string key = new Kullanici().SifirlamaKey(eMail);

            if(key.Length <= 1)
                return RedirectToAction("ForgotPass", new { hata = "Girdiğiniz mail adresine kayıtlı kullanıcı bulunamamıştır." });

            return RedirectToAction("ForgotPass", new { hata = "Sıfırlama mailinizi gönderdik lütfen gelen maildeki linke tıklayarak şifrenizi yenileyiniz." });
        }

        public ActionResult Blog(int id = 1)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult BlogDetail(int id = 1)
        {
            ViewBag.id = id;

            if (new Blog().IsBlog(id))
                return View();
            else
                return RedirectToAction("Index");
        }

        public ActionResult Product()
        {
            return View();
        }

        public ActionResult ProductDetail()
        {
            return View();
        }
        #region Ajax Data
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult FavoriEkle(int id)
        {
            Kullanici musteri = Site.GetMusteri();

            if(musteri != null)
            {
                Urun urun = new Urun();
                urun.LoadFromID(id);
                bool oldu = Site.SetFavori(urun);
                return Json(new { resim = (urun.urunResimleri.Count > 0 ? urun.urunResimleri[0].resim : "yok.jpg"), durum = (oldu ? "true" : "false"), login = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { resim = "yok.jpg", durum = "false", login = false }, JsonRequestBehavior.AllowGet);
            }

        }

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SepetEkle(int id, int miktar, int ekleCikar)
        {
            Urun urun = new Urun();
            urun.LoadFromID(id);
            Site.SetSepet(urun, miktar, ekleCikar == 1);
            int count = Site.GetSepet().Count;
            return Json(new { urun = urun, miktar = miktar, durum = (ekleCikar == 1 ? "true" : "false"), count = count }, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult HizliGoster(int id)
        {
            Urun urun = new Urun();
            urun.LoadFromID(id);
            return Json(new { urun = urun }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SepetGuncelle()
        {
            return PartialView("~/Views/Shared/_Sepet.cshtml");
        }

        [HttpPost]
        public ActionResult WishlistGuncelle()
        {
            return PartialView("~/Views/Shared/_Wishlist.cshtml");
        }

        [HttpPost]
        public JsonResult MesajGonder(string adSoyad, string eMail, string telefon, string baslik, string kaynakEkran, string mesaj)
        {
            string hata = "";

            if (mesaj.Length <= 0)
            {
                hata = "Lütfen, mesaj girin!";
                return Json(new { basarili = false, hata = hata }, JsonRequestBehavior.AllowGet);
            }
            if (baslik.Length <= 0)
            {
                hata = "Lütfen, başlık girin!";
                return Json(new { basarili = false, hata = hata }, JsonRequestBehavior.AllowGet);
            }
            if (eMail.Length <= 0)
            {
                hata = "Lütfen, e-mail girin!";
                return Json(new { basarili = false, hata = hata }, JsonRequestBehavior.AllowGet);
            }
            if (adSoyad.Length <= 0)
            {
                hata = "Lütfen, ad soyad girin!";
                return Json(new { basarili = false, hata = hata }, JsonRequestBehavior.AllowGet);
            }

            var response = Request["g-recaptcha-response"];
            string secret = ConfigurationManager.AppSettings["recaptcha_secret"];

            var client = new WebClient();
            var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            if (captchaResponse.Success)
            {
                new Mesaj().Kaydet(adSoyad, eMail, telefon, baslik, mesaj, kaynakEkran);
            }
            else
                hata = "Lütfen, robot değlim seçeneğini işaretleyin!";

            return Json(new { basarili = captchaResponse.Success, hata = hata }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}