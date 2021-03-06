﻿using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Newtonsoft.Json;
using sCommerce.Helper;
using sCommerce.Models;
using System;
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

        public ActionResult Wishlist()
        {
            return View();
        }

        public ActionResult Cart()
        {
            return View();
        }

        public ActionResult CheckOut()
        {
            return View();
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

        public ActionResult Category(int id = 0, int markaID = 0, int modelGrubuID = 0, int siralama = 0, int count = 18)
        {
            Kategori k = new Kategori();

            if (id == 0)
            {
                k.kategoriID = 0;
                k.kategori = "Mağaza";
                k.resim = "";
                k.ustKategoriID = 0;
                k.loadAltKategori();
            }
            else
            {
                if (!k.LoadKategori(id))
                    return RedirectToAction("Index");
            }

            ViewBag.kategori = k;
            ViewBag.siralama = siralama;
            ViewBag.count = count;
            ViewBag.markaID = markaID;
            ViewBag.modelGrubuID = modelGrubuID;

            return View();
        }

        public ActionResult SiparisGir(int adresID, int odemeTipi, string siparisNotu)
        {
            if (Site.GetMusteri() == null)
                return RedirectToAction("Login");

            if (new Sepet().DegisiklikVarmi())
                return RedirectToAction("CheckOut", new { hata = "Sepetinizde stoğu bitmiş ürünler var, sepetiniz düzenlenmiştir.<br/> Siparişinize devam edebilirsiniz." });

            int siparisID = new Siparis().Kaydet(adresID, odemeTipi, siparisNotu);
            if(siparisID == 0)
                return RedirectToAction("CheckOut", new { hata = "Bir hata oluştu, lütfen tekrar deneyiniz." });

            return RedirectToAction("Pay", new { id = siparisID });
        }

        public ActionResult Order(int id)
        {
            Kullanici musteri = Site.GetMusteri();
            if (musteri == null)
                return RedirectToAction("Login");

            ViewBag.id = id;
            return View();
        }

        public ActionResult Pay(int id)
        {
            ViewBag.id = id;
            Siparis siparis = new Siparis();
            siparis.Load(id);
            ViewBag.siparis = siparis;

            Kullanici musteri = new Kullanici();
            musteri.load(siparis.kullaniciID);

            if(siparis.odemeTipi == (int)Helper.odemeTipleri.krediBankaKarti)
            {
                string siteUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

                Options options = new Options();
                options.ApiKey = ConfigurationManager.AppSettings["iyzico_apikey"];
                options.SecretKey = ConfigurationManager.AppSettings["iyzico_secretkey"];
                options.BaseUrl = ConfigurationManager.AppSettings["iyzico_baseurl"];

                CreateCheckoutFormInitializeRequest request = new CreateCheckoutFormInitializeRequest();
                request.Locale = Locale.TR.ToString();
                request.ConversationId = id.ToString();
                request.Price = (siparis.GetToplamFiyat() - siparis.kargoUcreti).ToString().Replace(',', '.');
                request.PaidPrice = siparis.GetToplamFiyat().ToString().Replace(',', '.');
                request.Currency = Currency.TRY.ToString();
                request.BasketId = siparis.siparisID.ToString();
                request.PaymentGroup = PaymentGroup.PRODUCT.ToString();
                request.CallbackUrl = siteUrl + "/Home/PayOut";

                List<int> enabledInstallments = new List<int>();
                enabledInstallments.Add(2);
                enabledInstallments.Add(3);
                enabledInstallments.Add(6);
                enabledInstallments.Add(9);
                request.EnabledInstallments = enabledInstallments;

                Buyer buyer = new Buyer();
                buyer.Id = siparis.kullaniciID.ToString();
                buyer.Name = musteri.ad;
                buyer.Surname = musteri.soyad;
                buyer.GsmNumber = siparis.telefon;
                buyer.Email = musteri.eMail;
                buyer.IdentityNumber = "74300864791";
                //buyer.LastLoginDate = "2015-10-05 12:43:35";
                //buyer.RegistrationDate = "2013-04-21 15:12:09";
                buyer.RegistrationAddress = siparis.adresSatir1 + " " + siparis.adresSatir2;
                buyer.Ip = SQL.GetIp();
                buyer.City = siparis.sehir;
                buyer.Country = "Turkey";
                buyer.ZipCode = siparis.postaKodu;
                request.Buyer = buyer;

                Address shippingAddress = new Address();
                shippingAddress.ContactName = siparis.ad + " " + siparis.soyad;
                shippingAddress.City = siparis.sehir;
                shippingAddress.Country = "Turkey";
                shippingAddress.Description = siparis.adresSatir1 + " " + siparis.adresSatir2;
                shippingAddress.ZipCode = siparis.postaKodu;
                request.ShippingAddress = shippingAddress;

                Address billingAddress = new Address();
                billingAddress.ContactName = siparis.ad + " " + siparis.soyad;
                billingAddress.City = siparis.sehir;
                billingAddress.Country = "Turkey";
                billingAddress.Description = siparis.adresSatir1 + " " + siparis.adresSatir2;
                billingAddress.ZipCode = siparis.postaKodu;
                request.BillingAddress = billingAddress;

                List<BasketItem> basketItems = new List<BasketItem>();
                foreach (SiparisKalem siparisKalem in siparis.siparisKalemleri)
                {
                    BasketItem firstBasketItem = new BasketItem();
                    firstBasketItem.Id = siparisKalem.siparisKalemID.ToString();
                    firstBasketItem.Name = siparisKalem.urun.urunAdi;
                    firstBasketItem.Category1 = siparisKalem.urun.modelGrubu.modelGrubu;
                    firstBasketItem.Category2 = siparisKalem.urun.marka.marka;
                    firstBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                    firstBasketItem.Price = (siparisKalem.miktar * siparisKalem.fiyat).ToString().Replace(',', '.');
                    basketItems.Add(firstBasketItem);
                }
                request.BasketItems = basketItems;

                CheckoutFormInitialize checkoutFormInitialize = CheckoutFormInitialize.Create(request, options);
                ViewBag.Iyzico = checkoutFormInitialize.CheckoutFormContent; //View Dönüş yapılan yer, Burada farklı yöntemler ile View gönderim yapabilirsiniz.
            }

            return View();
        }

        public ActionResult PayOut(RetrieveCheckoutFormRequest model)
        {
            int siparisID = 0;

            string data = "";
            Options options = new Options();
            options.ApiKey = ConfigurationManager.AppSettings["iyzico_apikey"];
            options.SecretKey = ConfigurationManager.AppSettings["iyzico_secretkey"];
            options.BaseUrl = ConfigurationManager.AppSettings["iyzico_baseurl"];
            data = model.Token;
            RetrieveCheckoutFormRequest request = new RetrieveCheckoutFormRequest();
            request.Token = data;
            CheckoutForm checkoutForm = CheckoutForm.Retrieve(request, options);

            Int32.TryParse(checkoutForm.BasketId, out siparisID);
            Siparis siparis = new Siparis();
            siparis.Load(siparisID);

            if (checkoutForm.PaymentStatus == "SUCCESS")
            {
                siparis.DurumGuncelle((int)siparisDurum.hazirlaniyor, 0);
            }
            else
            {
                siparis.DurumGuncelle((int)siparisDurum.odemeBasarisiz, 0);
            }

            return RedirectToAction("Pay", new { id = siparisID });
        }

        public ActionResult Product(int id = 0)
        {
            if(id == 0)
                return RedirectToAction("Index");

            Urun u = new Urun();
            if(!u.LoadFromID(id))
                return RedirectToAction("Index");

            ViewBag.urun = u;

            return View();
        }
        #region Kullanıcı İşlemleri
        public ActionResult Customer()
        {
            if (Site.GetMusteri() == null)
                return RedirectToAction("Login");

            return View();
        }

        public ActionResult ChangePass()
        {
            if (Site.GetMusteri() == null)
                return RedirectToAction("Login");

            return View();
        }

        public ActionResult Adress()
        {
            if (Site.GetMusteri() == null)
                return RedirectToAction("Login");

            return View();
        }

        public ActionResult AdressDetail(int id = 0)
        {
            Kullanici musteri = Site.GetMusteri();
            if (musteri == null)
                return RedirectToAction("Login");

            ViewBag.id = id;
            return View();
        }

        public ActionResult Search(string searchText = "")
        {
            if (searchText.Length <= 2)
                return RedirectToAction("Index");

            ViewBag.searchText = searchText;
            List<Urun> uruns = new Urun().UrunAra(searchText);
            ViewBag.uruns = uruns;

            return View();
        }

        public ActionResult AdresEkle(int adresID, string adres, string ad, string soyad, string telefon, string sehir, string semt, string mahalle, string adresSatir1, string adresSatir2, string postaKodu)
        {
            Kullanici musteri = Site.GetMusteri();
            if (musteri == null)
                return RedirectToAction("Login");

            new Adres().AdresEkleGuncelle(adresID, adres, ad, soyad, telefon, sehir, semt, mahalle, adresSatir1, adresSatir2, postaKodu, musteri.kullaniciID);
            musteri.adresler = new Adres().GetKullaniciAdres(musteri.kullaniciID);
            return RedirectToAction("Adress", new { hata = "Adres düzenlenmiştir." });
        }

        public ActionResult ForgotPass()
        {
            if (Site.GetMusteri() != null)
                return RedirectToAction("Index");

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

        public ActionResult Login(int id = 0)
        {
            if (Site.GetMusteri() != null)
                return RedirectToAction("Index");
            ViewBag.id = id;

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

        public ActionResult SifreGuncelle(string eMail, string sifre, string eskiSifre)
        {
            Kullanici musteri = new Kullanici();
            string hata = musteri.GirisKontrol(eMail, eskiSifre);

            if(hata.Length > 1)
                return RedirectToAction("Customer", new { hata = "Eski şifreniz yanlış olduğu için güncelleme yapılamadı, sıfırlamayı deneyebilirsiniz." });

            musteri.SifreGuncelle(eMail, sifre, eskiSifre);
            return RedirectToAction("Customer", new { hata = "Şifreniz güncellenmiştir, yeni şifreniz ile giriş yapabilirsiniz." });
        }

        public ActionResult CikisYap()
        {
            Session["musteri"] = null;
            Site.CookieDelete("eMail");
            Site.CookieDelete("sifre");

            return RedirectToAction("Index");
        }

        public ActionResult GirisYap(string eMail, string sifre, int id = 0)
        {
            Kullanici musteri = new Kullanici();
            string hata = musteri.GirisKontrol(eMail, sifre);

            if (hata.Length > 1)
                return RedirectToAction("Login", new { hata = hata, id = id });

            Session["musteri"] = musteri;
            Site.CookieCreate("eMail", musteri.eMail);
            Site.CookieCreate("sifre", musteri.sifre);

            return RedirectToAction(id == 1 ? "CheckOut" : "Index");
        }

        public ActionResult UyeOl(string ad, string soyad, string eMail, string sifre, int id = 0)
        {
            if (ad.Length <= 0)
                return RedirectToAction("Login", new { hata = "Lütfen adınızı girin.", id = id });
            if (soyad.Length <= 0)
                return RedirectToAction("Login", new { hata = "Lütfen soyadınızı girin.", id = id });
            if (eMail.Length <= 0)
                return RedirectToAction("Login", new { hata = "Lütfen mail adresinizi girin.", id = id });
            if (sifre.Length <= 5)
                return RedirectToAction("Login", new { hata = "Lütfen şifrenizi girin.", id = id });

            Kullanici musteri = new Kullanici();
            string hata = musteri.Kaydet(ad, soyad, sifre, eMail);

            if (hata.Length > 1)
                return RedirectToAction("Login", new { hata = hata, id = id });

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
        #endregion
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

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult AdresGetir(int adresID = 0)
        {
            Adres adres = new Adres();
            adres.LoadFromID(adresID);
            return Json(new { adres = adres }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SepetGuncelle()
        {
            return PartialView("~/Views/Shared/_Sepet.cshtml");
        }

        [HttpPost]
        public ActionResult CartGuncelle()
        {
            return PartialView("~/Views/Shared/_Cart.cshtml");
        }

        [HttpPost]
        public ActionResult AlisverisiTamamlaGuncelle()
        {
            return PartialView("~/Views/Shared/_AlisverisiTamamla.cshtml");
        }

        [HttpPost]
        public ActionResult WishlistGuncelle()
        {
            return PartialView("~/Views/Shared/_Wishlist.cshtml");
        }

        public ActionResult DevaminiYukle()
        {
            UrunFilter uf = (UrunFilter)Session["urunFilter"];
            uf.page++;
            Session["urunFilter"] = uf;
            return PartialView("~/Views/Shared/_Category.cshtml");
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
        #region Extra Sayfalar
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

        #endregion
    }
}