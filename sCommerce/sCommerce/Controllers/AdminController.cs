using sCommerce.Helper;
using sCommerce.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Linq;

namespace sCommerce.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            if (Session["kullaniciID"] == null)
                return RedirectToAction("Login");
            return View();
        }

        public ActionResult ComingSoon()
        {
            return RedirectToAction("Index");
        }

        #region Login
        public ActionResult Login()
        {
            if (Session["kullaniciID"] != null)
                return RedirectToAction("Index");
            return View();
        }

        public ActionResult kullaniciGiris(string eMail, string sifre)
        {
            if (eMail.Length <= 0 || sifre.Length <= 0)
                return RedirectToAction("Login", new { hata = "EMail ve Şifre giriniz!" });

            DataTable dt_kullanici = SQL.get("SELECT * FROM kullanicilar WHERE silindi = 0 AND eMail = '" + eMail + "' AND sifre = '" + SQL.MD5(sifre) + "'");
            if (dt_kullanici.Rows.Count > 0)
            {
                Session["kullaniciID"] = dt_kullanici.Rows[0]["kullaniciID"];
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Login", new { hata = "Kullanıcı bulunamadı..." });
        }

        public ActionResult kullaniciCikis()
        {
            Session["kullaniciID"] = null;
            return RedirectToAction("Index");
        }
        #endregion
        #region Kullanici
        public ActionResult Kullanici()
        {
            if (Session["kullaniciID"] == null)
                return RedirectToAction("Login");

            return View();
        }

        [HttpPost]
        public ActionResult kullaniciEkle(string ad, string soyad, string telefon, string eMail, string sifre, int kullaniciTipiParametreID)
        {
            if (ad.Length <= 0)
                return RedirectToAction("Kullanici", new { hata = "Ad giriniz!" });
            if (soyad.Length <= 0)
                return RedirectToAction("Kullanici", new { hata = "Soyad giriniz!" });
            if (eMail.Length <= 0)
                return RedirectToAction("Kullanici", new { hata = "EMail giriniz!" });
            if (sifre.Length <= 0)
                return RedirectToAction("Kullanici", new { hata = "Şifre giriniz!" });

            DataTable dt = SQL.get("SELECT * FROM kullanicilar WHERE silindi = 0 AND email = '" + eMail + "'");
            if (dt.Rows.Count > 0)
                return RedirectToAction("Kullanicilar", new { hata = "Girdiğiniz E-Mail kullanılmaktadır!" });

            SQL.set("INSERT INTO kullanicilar (kaydedenKullaniciID, eMail, sifre, ad, soyad, kullaniciTipiParametreID, telefon) VALUES (" + Session["kullaniciID"] + ", '" + eMail + "', '" + SQL.MD5(sifre) + "', '" + ad + "', '" + soyad + "', " + kullaniciTipiParametreID + ", '" + telefon + "')");

            return RedirectToAction("Kullanici", new { tepki = 1 });
        }

        [HttpPost]
        public ActionResult kullaniciDuzenle(int kullaniciID, string ad, string soyad, string telefon, string eMail, string sifre, int kullaniciTipiParametreID)
        {
            if (ad.Length <= 0)
                return RedirectToAction("Kullanici", new { hata = "Ad giriniz!" });
            if (soyad.Length <= 0)
                return RedirectToAction("Kullanici", new { hata = "Soyad giriniz!" });
            if (eMail.Length <= 0)
                return RedirectToAction("Kullanici", new { hata = "EMail giriniz!" });
            if (sifre.Length <= 0)
                return RedirectToAction("Kullanici", new { hata = "Şifre giriniz!" });

            DataTable dt = SQL.get("SELECT * FROM kullanicilar WHERE silindi = 0 AND kullaniciID != " + kullaniciID + " AND eMail = '" + eMail + "'");
            if (dt.Rows.Count > 0)
                return RedirectToAction("Kullanicilar", new { hata = "Girdiğiniz E-Mail kullanılmaktadır!" });
            dt = SQL.get("SELECT * FROM kullanicilar WHERE silindi = 0 AND kullaniciID = " + kullaniciID);

            if (sifre == dt.Rows[0]["sifre"].ToString())
                sifre = "";

            SQL.set("UPDATE kullanicilar SET guncelleyenKullaniciID = " + Session["kullaniciID"] + ", guncellemeTarihi = GETDATE(), eMail = '" + eMail + "', ad = '" + ad + "', soyad = '" + soyad + "', kullaniciTipiParametreID = " + kullaniciTipiParametreID + ", telefon = '" + telefon + "' " + (sifre.Length > 0 ? ", sifre = '" + SQL.MD5(sifre) + "'" : " ") + " WHERE kullaniciID = " + kullaniciID);

            return RedirectToAction("Kullanici", new { tepki = 2 });
        }

        public ActionResult kullaniciSil(int kullaniciID)
        {
            SQL.set("UPDATE kullanicilar SET guncelleyenKullaniciID = " + Session["kullaniciID"] + ", guncellemeTarihi = GETDATE(), silindi = 1 WHERE kullaniciID = " + kullaniciID);
            return RedirectToAction("Kullanici", new { tepki = 3 });
        }
        #endregion
        #region SiteBilgi
        public ActionResult SiteBilgi()
        {
            if (Session["kullaniciID"] == null)
                return RedirectToAction("Login");
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult siteBilgiGuncelle(string siteAdi, string tel, string gsm, string whatsapp, string eMail, string calismaSaatleri, string adres, string seoAciklama, string seoKeywords, 
            string slogan, string hakkimizda, string fax, string facebook, string twitter, string instagram, string youtube, string linkedin, string pinterest, string uyelikSozlesmesi, string kullanimSartlari, 
            string mesafeliSatisSozlesmesi, string gizlilikPolitikasi, string sikSorulanSorular, HttpPostedFileBase logo, string iban, string banka, string hesapNo, string sube)
        {
            string result = "";
            if (logo != null && logo.ContentLength > 0)
            {
                result = string.Format(@"{0}", Guid.NewGuid());
                WebImage img = new WebImage(logo.InputStream);
                var path = Path.Combine("~/Content/icerik/site_logo/orjinal/", result);
                img.Save(path);
                img.Resize(300, 150, true, false).Crop(1, 1, 1, 1);
                path = Path.Combine("~/Content/icerik/site_logo/", result);
                img.Save(path);
                result += Path.GetExtension(logo.FileName);
                result = result.Replace("jpg", "jpeg");
            }
            string sql = "UPDATE siteBilgileri SET siteAdi = '" + siteAdi + "', tel = '" + tel + "', gsm = '" + gsm + "', whatsapp = '" + whatsapp + "', eMail = '" + eMail + "', calismaSaatleri = '" + calismaSaatleri + "', " + 
                " adres = '" + adres + "', seoAciklama = '" + seoAciklama + "', seoKeywords = '" + seoKeywords + "', slogan = '" + slogan + "', hakkimizda = '" + hakkimizda + "', fax = '" + fax + "', " + 
                " facebook = '" + facebook + "', twitter = '" + twitter + "', instagram = '" + instagram + "', youtube = '" + youtube + "', linkedin = '" + linkedin + "', pinterest = '" + pinterest + "', " + 
                " uyelikSozlesmesi = '" + uyelikSozlesmesi + "', kullanimSartlari = '" + kullanimSartlari + "', mesafeliSatisSozlesmesi = '" + mesafeliSatisSozlesmesi + "', gizlilikPolitikasi = '" + gizlilikPolitikasi + "', " + 
                " sikSorulanSorular = '" + sikSorulanSorular + "', logo = " + (result.Length <= 0 ? "logo" : "'" + result + "'") + ", iban = '" + iban + "', banka = '" + banka + "', hesapNo = '" + hesapNo + "', " + 
                " sube = '" + sube + "'";

            SQL.set(sql);
            Site.siteBilgileri.load();
            return RedirectToAction("SiteBilgi", new { tepki = 1 });
        }
        #endregion
        #region Kategori
        public ActionResult Kategori(int id = 0)
        {
            if (Session["kullaniciID"] == null)
                return RedirectToAction("Login");
            if(id == 0)
                return RedirectToAction("Index");
            ViewBag.kategoriTipiParametreID = id;

            return View();
        }

        [HttpPost]
        public ActionResult kategoriEkle(string kategori, int kategoriTipiParametreID, int ustKategoriID, string ikon, HttpPostedFileBase resim, string aciklama)
        {
            if (kategori.Length <= 0)
                return RedirectToAction("Kategori", new { hata = "Kategori adı giriniz!" });

            string result = "";
            if (resim != null && resim.ContentLength > 0)
            {
                result = string.Format(@"{0}", Guid.NewGuid());
                WebImage img = new WebImage(resim.InputStream);
                var path = Path.Combine("~/Content/icerik/kategori/orjinal/", result);
                img.Save(path);
                img.Resize(250, 250, true, false).Crop(1, 1, 1, 1);
                path = Path.Combine("~/Content/icerik/kategori/", result);
                img.Save(path);
                result += Path.GetExtension(resim.FileName);
                result = result.Replace("jpg", "jpeg");
            }

            SQL.set("INSERT INTO kategoriler (kaydedenKullaniciID, kategori, kategoriTipiParametreID, ustKategoriID, ikon, resim, aciklama) VALUES (" + Session["kullaniciID"] + ", '" + kategori + "', " + kategoriTipiParametreID + ", " + ustKategoriID + ", '" + ikon + "', '" + result + "', '" + aciklama + "')");

            if (kategoriTipiParametreID == 4)
                Site.urunKategorileri = new Kategori().loadKategoriler(kategoriTipiParametreID);
            else if (kategoriTipiParametreID == 3)
                Site.blogKategorileri = new Kategori().loadKategoriler(kategoriTipiParametreID);

            return RedirectToAction("Kategori", new { tepki = 1, id = kategoriTipiParametreID });
        }

        [HttpPost]
        public ActionResult kategoriDuzenle(int kategoriID, string kategori, int kategoriTipiParametreID, int ustKategoriID, string ikon, HttpPostedFileBase resim, string aciklama)
        {
            if (kategori.Length <= 0)
                return RedirectToAction("Kategori", new { hata = "Kategori adı giriniz!" });

            string result = "";
            if (resim != null && resim.ContentLength > 0)
            {
                result = string.Format(@"{0}", Guid.NewGuid());
                WebImage img = new WebImage(resim.InputStream);
                var path = Path.Combine("~/Content/icerik/kategori/orjinal/", result);
                img.Save(path);
                img.Resize(250, 250, true, false).Crop(1, 1, 1, 1);
                path = Path.Combine("~/Content/icerik/kategori/", result);
                img.Save(path);
                result += Path.GetExtension(resim.FileName);
                result = result.Replace("jpg", "jpeg");
            }

            SQL.set("UPDATE kategoriler SET guncelleyenKullaniciID = " + Session["kullaniciID"] + ", guncellemeTarihi = GETDATE(), kategori = '" + kategori + "', kategoriTipiParametreID = " + kategoriTipiParametreID + ", ustKategoriID = " + ustKategoriID + ", ikon = '" + ikon + "' " + (result.Length > 0 ? ", resim = '" + result + "' " : " ") + " , aciklama = '" + aciklama + "' WHERE kategoriID = " + kategoriID);

            if (kategoriTipiParametreID == 4)
                Site.urunKategorileri = new Kategori().loadKategoriler(kategoriTipiParametreID);
            else if (kategoriTipiParametreID == 3)
                Site.blogKategorileri = new Kategori().loadKategoriler(kategoriTipiParametreID);

            return RedirectToAction("Kategori", new { tepki = 2, id = kategoriTipiParametreID });
        }

        public ActionResult kategoriSil(int kategoriID, int kategoriTipiParametreID)
        {
            int cnt = 0;

            DataTable dt = SQL.get("SELECT * FROM bloglar WHERE silindi = 0 AND kategoriID = " + kategoriID);
            cnt += dt.Rows.Count;
            //dt = SQL.get("SELECT * FROM urunKategori WHERE silindi = 0 AND kategoriID = " + kategoriID);
            //cnt += dt.Rows.Count;

            if (cnt > 0)
                return RedirectToAction("Kategori", new { hata = "Silmek istediğiniz kategoriye bağlı kayıtlar vardır, bu kayıtlar mevcutken kategori silinemez!" });
            SQL.set("UPDATE urunKategori SET guncelleyenKullaniciID = " + Session["kullaniciID"] + ", guncellemeTarihi = GETDATE(), silindi = 1 WHERE kategoriID = " + kategoriID);
            SQL.set("UPDATE kategoriler SET guncelleyenKullaniciID = " + Session["kullaniciID"] + ", guncellemeTarihi = GETDATE(), silindi = 1 WHERE kategoriID = " + kategoriID);
            
            if (kategoriTipiParametreID == 4)
                Site.urunKategorileri = new Kategori().loadKategoriler(kategoriTipiParametreID);
            else if (kategoriTipiParametreID == 3)
            Site.blogKategorileri = new Kategori().loadKategoriler(kategoriTipiParametreID);

            return RedirectToAction("Kategori", new { tepki = 3, id = kategoriTipiParametreID });
        }
        #endregion
        #region Blog
        public ActionResult Blog()
        {
            if (Session["kullaniciID"] == null)
                return RedirectToAction("Login");
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult blogEkle(HttpPostedFileBase gorsel, int kategoriID, string baslik, string yazi)
        {
            if (baslik.Length <= 0)
                return RedirectToAction("Blog", new { hata = "Eksik bilgi girdiniz!" });

            string result = "";
            if (gorsel != null && gorsel.ContentLength > 0)
            {
                result = string.Format(@"{0}", Guid.NewGuid());
                WebImage img = new WebImage(gorsel.InputStream);
                var path = Path.Combine("~/Content/icerik/blog/orjinal/", result);
                img.Save(path);
                img.Resize(1600, 1600, true, false).Crop(1, 1, 1, 1);
                path = Path.Combine("~/Content/icerik/blog/buyuk/", result);
                img.Save(path);
                img.Resize(400, 400, true, false).Crop(1, 1, 1, 1);
                path = Path.Combine("~/Content/icerik/blog/kucuk/", result);
                img.Save(path);
                result += Path.GetExtension(gorsel.FileName);
                result = result.Replace("jpg", "jpeg");
            }

            SQL.set("INSERT INTO bloglar (kaydedenKullaniciID, kategoriID, baslik, gorsel, yazi) VALUES (" + Session["kullaniciID"] + ", " + kategoriID + ", '" + baslik + "', '" + result + "', '" + yazi + "')");

            return RedirectToAction("Blog", new { tepki = 1 });
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult blogDuzenle(int blogID, HttpPostedFileBase gorsel, int kategoriID, string baslik, string yazi)
        {
            if (baslik.Length <= 0)
                return RedirectToAction("Blog", new { hata = "Eksik bilgi girdiniz!" });

            string result = "";
            if (gorsel != null && gorsel.ContentLength > 0)
            {
                result = string.Format(@"{0}", Guid.NewGuid());
                WebImage img = new WebImage(gorsel.InputStream);
                var path = Path.Combine("~/Content/icerik/blog/orjinal/", result);
                img.Save(path);
                img.Resize(1600, 1600, true, false).Crop(1, 1, 1, 1);
                path = Path.Combine("~/Content/icerik/blog/buyuk/", result);
                img.Save(path);
                img.Resize(400, 400, true, false).Crop(1, 1, 1, 1);
                path = Path.Combine("~/Content/icerik/blog/kucuk/", result);
                img.Save(path);
                result += Path.GetExtension(gorsel.FileName);
                result = result.Replace("jpg", "jpeg");
            }

            SQL.set("UPDATE bloglar SET guncelleyenKullaniciID = " + Session["kullaniciID"] + ", guncellemeTarihi = GETDATE(), kategoriID = " + kategoriID + ", baslik = '" + baslik + "', yazi = '" + yazi + "' " + (result.Length <= 0 ? "" : ", gorsel = '" + result + "' ") + " WHERE blogID = " + blogID);

            return RedirectToAction("Blog", new { tepki = 1 });
        }

        public ActionResult blogSil(int blogID)
        {
            SQL.set("UPDATE bloglar SET guncelleyenKullaniciID = " + Session["kullaniciID"] + ", guncellemeTarihi = GETDATE(), silindi = 1 WHERE blogID = " + blogID);
            return RedirectToAction("Blog", new { tepki = 3 });
        }
        #endregion
        #region Ürün Özellik
        public ActionResult UrunOzellik()
        {
            if (Session["kullaniciID"] == null)
                return RedirectToAction("Login");

            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ozellikEkle(string ozellik)
        {
            if (ozellik.Length <= 0)
                return RedirectToAction("UrunOzellik", new { hata = "Eksik bilgi girdiniz!" });

            SQL.set("INSERT INTO ozellikler (kaydedenKullaniciID, ozellik) VALUES (" + Session["kullaniciID"] + ", '" + ozellik + "')");

            return RedirectToAction("UrunOzellik", new { tepki = 1 });
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ozellikDuzenle(int ozellikID, string ozellik)
        {
            if (ozellik.Length <= 0)
                return RedirectToAction("UrunOzellik", new { hata = "Eksik bilgi girdiniz!" });

            SQL.set("UPDATE ozellikler SET guncelleyenKullaniciID = " + Session["kullaniciID"] + ", guncellemeTarihi = GETDATE(), ozellik = '" + ozellik + "' WHERE ozellikID = " + ozellikID);

            return RedirectToAction("UrunOzellik", new { tepki = 1 });
        }

        public ActionResult ozellikSil(int ozellikID)
        {
            SQL.set("UPDATE ozellikler SET guncelleyenKullaniciID = " + Session["kullaniciID"] + ", guncellemeTarihi = GETDATE(), silindi = 1 WHERE ozellikID = " + ozellikID);
            return RedirectToAction("UrunOzellik", new { tepki = 3 });
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ozellikDegerEkle(int ozellikID, string deger)
        {
            if (deger.Length <= 0)
                return RedirectToAction("UrunOzellik", new { hata = "Eksik bilgi girdiniz!" });

            SQL.set("INSERT INTO ozellikDegerleri (kaydedenKullaniciID, deger, ozellikID) VALUES (" + Session["kullaniciID"] + ", '" + deger + "', " + ozellikID + ")");

            return RedirectToAction("UrunOzellik", new { tepki = 1 });
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ozellikDegerDuzenle(int ozellikDegerID, string deger)
        {
            if (deger.Length <= 0)
                return RedirectToAction("UrunOzellik", new { hata = "Eksik bilgi girdiniz!" });

            SQL.set("UPDATE ozellikDegerleri SET guncelleyenKullaniciID = " + Session["kullaniciID"] + ", guncellemeTarihi = GETDATE(), deger = '" + deger + "' WHERE ozellikDegerID = " + ozellikDegerID);

            return RedirectToAction("UrunOzellik", new { tepki = 1 });
        }

        public ActionResult ozellikDegerSil(int ozellikDegerID)
        {
            SQL.set("UPDATE ozellikDegerleri SET guncelleyenKullaniciID = " + Session["kullaniciID"] + ", guncellemeTarihi = GETDATE(), silindi = 1 WHERE ozellikDegerID = " + ozellikDegerID);
            return RedirectToAction("UrunOzellik", new { tepki = 3 });
        }

        [HttpPost]
        public ActionResult ozellikDegerGetir(int ozellikID)
        {
            string ret_data = "";
            DataTable dt = SQL.get("SELECT * FROM ozellikDegerleri WHERE silindi = 0 AND ozellikID = " + ozellikID);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ret_data += "<option value='" + dt.Rows[i]["ozellikDegerID"] + "'>" + dt.Rows[i]["deger"] + "</option>";
            }

            return Json(new { result = ozellikID, message = ret_data });
        }
        #endregion
        #region Ürün
        public ActionResult Urun(int id = 0)
        {
            if (Session["kullaniciID"] == null)
                return RedirectToAction("Login");

            UrunFilter urunFilter;
            if(Session["AdminUrunFilter"] == null)
            {
                urunFilter = new UrunFilter("", new List<int>(), new List<int>(), new List<int>(), new List<int>(), filtremeleTipleri.urunAdi, false, (id <= 0 ? 1 : id), 20);
            }
            else
            {
                urunFilter = (UrunFilter)Session["AdminUrunFilter"];
                urunFilter.page = (id <= 0 ? urunFilter.page : id);
                urunFilter.count = 20;
            }
            Session["AdminUrunFilter"] = urunFilter;

            ViewBag.urunFilter = urunFilter;

            return View();
        }

        public ActionResult UrunTopluGuncelle(int kategoriIDEkle = -1, int kategoriIDCikar = -1, int oneCikan = -1, int markaID = -1, int urunEtiketiParametreID = -1, int urunDurumuParametreID = -1, int modelGrubuID = -1, 
            string depoLokasyonu = "", int stokBitinceParametreID = -1, int miktar = -1, int minimumMiktar = -1, string fiyat = "", string eskiFiyat = "", int kargoSuresi = -1, string agirlik = "", int vergiParametreID = -1,
            int vergiDahilSatis = -1)
        {
            if (Session["AdminUrunFilter"] == null)
            {
                return RedirectToAction("Urun", new { hata = "Toplu güncelleme yapmak için filtreleme yapmanız gerekmektedir." });
            }
            else
            {
                UrunFilter urunFilter = (UrunFilter)Session["AdminUrunFilter"];

                if(urunFilter.urunAdi == "" && urunFilter.kategoriIDs.Count <= 0 && urunFilter.markaIDs.Count <= 0 && urunFilter.modelGrubuIDs.Count <= 0 && urunFilter.oneCikanlar == false && urunFilter.urunEtiketParametreIDs.Count <= 0)
                    return RedirectToAction("Urun", new { hata = "Toplu güncelleme yapmak için filtreleme yapmanız gerekmektedir." });

                urunFilter.page = 1;
                urunFilter.count = 1000;
                List<Urun> uruns = urunFilter.GetUruns();
                string urunIDs = string.Join(",", uruns.Select(x => x.urunID));

                if (oneCikan != -1 || markaID != -1 || urunEtiketiParametreID != -1 || urunDurumuParametreID != -1 || modelGrubuID != -1 || depoLokasyonu != "" || stokBitinceParametreID != -1 || miktar != -1 || fiyat != "" || eskiFiyat != "" 
                    || kargoSuresi != -1 || agirlik != "" || vergiParametreID != -1 || vergiDahilSatis != -1)
                {
                    SQL.set("UPDATE urunler SET guncelleyenKullaniciID = " + Session["kullaniciID"] + ", guncellemeTarihi = GETDATE() " +
                        (oneCikan != -1 ? ", oneCikanlar = " + oneCikan : "") +
                        (markaID != -1 ? ", markaID = " + markaID : "") +
                        (urunEtiketiParametreID != -1 ? ", urunEtiketiParametreID = " + urunEtiketiParametreID : "") +
                        (urunDurumuParametreID != -1 ? ", urunDurumuParametreID = " + urunDurumuParametreID : "") +
                        (modelGrubuID != -1 ? ", modelGrubuID = " + modelGrubuID : "") +
                        (depoLokasyonu != "" ? ", depoLokasyonu = '" + depoLokasyonu + "'" : "") +
                        (stokBitinceParametreID != -1 ? ", stokBitinceParametreID = " + stokBitinceParametreID : "") +
                        (miktar != -1 ? ", miktar = " + miktar : "") +
                        (minimumMiktar != -1 ? ", minimumMiktar = " + minimumMiktar : "") +
                        (fiyat != "" ? ", fiyat = " + fiyat.ToString().Replace(',', '.') : "") +
                        (eskiFiyat != "" ? ", eskiFiyat = " + eskiFiyat.ToString().Replace(',', '.') : "") +
                        (kargoSuresi != -1 ? ", kargoSuresi = " + kargoSuresi : "") +
                        (agirlik != "" ? ", agirlik = " + agirlik.ToString().Replace(',', '.') : "") +
                        (vergiParametreID != -1 ? ", vergiParametreID = " + vergiParametreID : "") +
                        (vergiDahilSatis != -1 ? ", vergiDahilSatis = " + vergiDahilSatis : "") +
                        " WHERE urunID IN (" + urunIDs + ")");
                }

                if (kategoriIDCikar != -1)
                {
                    SQL.set("UPDATE urunKategori SET guncelleyenKullaniciID = " + Session["kullaniciID"] + ", guncellemeTarihi = GETDATE(), silindi = 1 WHERE kategoriID = " + kategoriIDCikar + " AND urunID IN (" + urunIDs + ")");
                }

                if (kategoriIDEkle != -1)
                {
                    SQL.set("INSERT INTO urunKategori (kaydedenKullaniciID, urunID, kategoriID) " +
                        "SELECT kaydedenKullaniciID = " + Session["kullaniciID"] + ", u.urunID, kategoriID = " + kategoriIDEkle + " FROM urunler u LEFT OUTER JOIN urunKategori uk ON uk.silindi = 0 AND uk.urunID = u.urunID AND uk.kategoriID = " + kategoriIDEkle + " WHERE u.silindi = 0 AND u.urunID IN (" + urunIDs + ") AND uk.urunKategoriID IS NULL");
                }
                

                return RedirectToAction("Urun", new { hata = "Toplu güncelleme başarılı." + uruns.Count + " adet ürün güncellendi." });
            }
        }

        public ActionResult UrunFiltrele(string urunAdi = "", int kategoriID = 0, int markaID = 0, int urunEtiket = 0, int modelGrubuID = 0, int oneCikanlar = 0)
        {
            if (Session["kullaniciID"] == null)
                return RedirectToAction("Login");

            UrunFilter urunFilter;
            if (Session["AdminUrunFilter"] == null)
            {
                urunFilter = new UrunFilter("", new List<int>(), new List<int>(), new List<int>(), new List<int>(), filtremeleTipleri.urunAdi, false, 1, 20);
            }
            else
            {
                urunFilter = (UrunFilter)Session["AdminUrunFilter"];
            }
            urunFilter.page = 1;
            urunFilter.urunAdi = urunAdi;
            urunFilter.kategoriIDs = (kategoriID == 0 ? new List<int>(): new List<int>() { kategoriID });
            urunFilter.markaIDs = (markaID == 0 ? new List<int>() : new List<int>() { markaID });
            urunFilter.urunEtiketParametreIDs = (urunEtiket == 0 ? new List<int>() : new List<int>() { urunEtiket });
            urunFilter.modelGrubuIDs = (modelGrubuID == 0 ? new List<int>() : new List<int>() { modelGrubuID });
            urunFilter.oneCikanlar = oneCikanlar == 1;
            Session["AdminUrunFilter"] = urunFilter;

            return RedirectToAction("Urun");
        }

        public ActionResult UrunBarkod(string barkod)
        {
            int urunID = new Urun().BarkodAra(barkod);

            if (urunID == 0)
            {
                return RedirectToAction("Urun", new { hata = "Girdiğiniz barkod ile ürün bulunamadı!" });

            }

            return RedirectToAction("UrunDetay", new { id = urunID, turet = 0 });
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult urunEkle(int[] kategoriID, int[] ozellikID, string[] resimPath, int[] resimID, int[] resimSira, string urunAdi = "", int urunDurumuParametreID = 0, int oneCikanlar = 0, string urunAciklamasi = "", string seoAciklama = "", string seoKeywords = "", int urunEtiketiParametreID = 0, int modelGrubuID = 0, string barkod = "", string stokKodu = "", string depoLokasyonu = "",
            string eskiFiyat = "0", string fiyat = "0", int vergiParametreID = 0, int vergiDahilSatis = 0, int miktar = 0, int minimumMiktar = 0, int stokBitinceParametreID = 0, string agirlik = "0", int kargoSuresi = 0, int markaID = 0)
        {
            string result = "";
            int yeni_urun_id;
            yeni_urun_id = Convert.ToInt32(SQL.get(
                "INSERT INTO urunler (kaydedenKullaniciID, urunAdi, urunAciklamasi, seoAciklama, seoKeywords, urunEtiketiParametreID, modelGrubuID, barkod, stokKodu, depoLokasyonu, eskiFiyat, fiyat, vergiParametreID, " + 
                " vergiDahilSatis, miktar, minimumMiktar, stokBitinceParametreID, agirlik, kargoSuresi, urunDurumuParametreID, oneCikanlar, markaID) " + 
                " VALUES (" + Session["kullaniciID"] + ", '" + urunAdi + "', '" + urunAciklamasi + "', '" + seoAciklama + "', '" + seoKeywords + "', " + urunEtiketiParametreID + ", " + modelGrubuID + ", '" + barkod + "'," + 
                " '" + stokKodu + "', '" + depoLokasyonu + "', " + eskiFiyat.ToString().Replace(',', '.') + ", " + fiyat.ToString().Replace(',', '.') + ", " + vergiParametreID + ", " + vergiDahilSatis + "," + 
                " " + miktar + ", " + minimumMiktar + ", " + stokBitinceParametreID + "," + agirlik.ToString().Replace(',', '.') + ", " + kargoSuresi + ", " + urunDurumuParametreID + ", " + oneCikanlar + ", " + markaID + "); SELECT SCOPE_IDENTITY();").Rows[0][0]);

            if (kategoriID != null)
            {
                for (int i = 0; i < kategoriID.Length; i++)
                {
                    SQL.set("INSERT INTO urunKategori (kaydedenKullaniciID, urunID, kategoriID) VALUES (" + Session["kullaniciID"] + ", " + yeni_urun_id + ", " + kategoriID[i] + ")");
                }
            }
            if (ozellikID != null)
            {
                for (int i = 0; i < ozellikID.Length; i++)
                {
                    SQL.set("INSERT INTO urunOzellik (kaydedenKullaniciID, urunID, degerID) VALUES (" + Session["kullaniciID"] + ", " + yeni_urun_id + ", " + ozellikID[i] + ")");
                }
            }
            if (resimID != null)
            {
                for (int i = 0; i < resimID.Length; i++)
                {
                    if (resimPath[i] != null && resimPath[i].Length > 0)
                    {
                        result = string.Format(@"{0}", Guid.NewGuid());
                        byte[] imageBytes = Convert.FromBase64String(resimPath[i].Replace("data:image/jpeg;base64,", ""));
                        WebImage img = new WebImage(imageBytes);
                        var path = Path.Combine("~/Content/icerik/urun/orjinal", result);
                        img.Save(path);
                        img.Resize(1600, 1600, true, false).Crop(1, 1, 1, 1);
                        path = Path.Combine("~/Content/icerik/urun/buyuk", result);
                        img.Save(path);
                        img.Resize(400, 400, true, false).Crop(1, 1, 1, 1);
                        path = Path.Combine("~/Content/icerik/urun/kucuk", result);
                        img.Save(path);
                        result += Path.GetExtension(img.FileName);
                        result = result.Replace("jpg", "jpeg");
                        SQL.set("INSERT INTO urunResim (kaydedenKullaniciID, urunID, resim, sira) VALUES (" + Session["kullaniciID"] + ", " + yeni_urun_id + ", '" + result + "', " + resimSira[i] + ")");
                    }

                }
            }

            return RedirectToAction("Urun", new { tepki = 1 });
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult urunDuzenle(int urunID, int[] kategoriID, int[] ozellikID, string[] resimPath, int[] resimID, int[] resimSira, string urunAdi = "", string urunAciklamasi = "", string seoAciklama = "", string seoKeywords = "", int urunEtiketiParametreID = 0, int modelGrubuID = 0, string barkod = "", string stokKodu = "", 
            string depoLokasyonu = "", string eskiFiyat = "0", string fiyat = "0", int vergiParametreID = 0, int vergiDahilSatis = 0, int miktar = 0, int minimumMiktar = 0, int stokBitinceParametreID = 0, string agirlik = "0", int kargoSuresi = 0, int urunDurumuParametreID = 0, 
            int oneCikanlar = 0, int markaID = 0)
        {
            string result = "";
            string resim_ids = "";
            SQL.set("UPDATE urunler SET guncelleyenKullaniciID = " + Session["kullaniciID"] + ", guncellemeTarihi = GETDATE(), urunAdi = '" + urunAdi + "', urunAciklamasi = '" + urunAciklamasi + "', " + 
                " seoAciklama = '" + seoAciklama + "', seoKeywords = '" + seoKeywords + "', urunEtiketiParametreID = " + urunEtiketiParametreID + ", modelGrubuID = " + modelGrubuID + ", barkod = '" + barkod + "', " + 
                " stokKodu = '" + stokKodu + "', depoLokasyonu = '" + depoLokasyonu + "', eskiFiyat = " + eskiFiyat.ToString().Replace(',', '.') + ", fiyat = " + fiyat.ToString().Replace(',', '.') + ", " + 
                " vergiParametreID = " + vergiParametreID + ", vergiDahilSatis = " + vergiDahilSatis + ", miktar = " + miktar + ", minimumMiktar = " + minimumMiktar + ", " + 
                " stokBitinceParametreID = " + stokBitinceParametreID + ", agirlik = " + agirlik.ToString().Replace(',', '.') + ", kargoSuresi = " + kargoSuresi + ", urunDurumuParametreID = " + urunDurumuParametreID + ", " + 
                " oneCikanlar = " + oneCikanlar + ", markaID = " + markaID + " WHERE urunID = " + urunID);

            SQL.set("UPDATE urunKategori SET guncelleyenKullaniciID = " + Session["kullaniciID"] + ", guncellemeTarihi = GETDATE(), silindi = 1 WHERE urunID = " + urunID);
            SQL.set("UPDATE urunOzellik SET guncelleyenKullaniciID = " + Session["kullaniciID"] + ", guncellemeTarihi = GETDATE(), silindi = 1 WHERE urunID = " + urunID);
            SQL.set("UPDATE urunVaryasyon SET guncelleyenKullaniciID = " + Session["kullaniciID"] + ", guncellemeTarihi = GETDATE(), silindi = 1 WHERE urunID = " + urunID);

            if (kategoriID != null)
            {
                for (int i = 0; i < kategoriID.Length; i++)
                {
                    SQL.set("INSERT INTO urunKategori (kaydedenKullaniciID, urunID, kategoriID) VALUES (" + Session["kullaniciID"] + ", " + urunID + ", " + kategoriID[i] + ")");
                }
            }
            if (ozellikID != null)
            {
                for (int i = 0; i < ozellikID.Length; i++)
                {
                    SQL.set("INSERT INTO urunOzellik (kaydedenKullaniciID, urunID, degerID) VALUES (" + Session["kullaniciID"] + ", " + urunID + ", " + ozellikID[i] + ")");
                }
            }
            if (resimID != null)
            {
                resim_ids = "";
                for (int i = 0; i < resimID.Length; i++)
                {
                    SQL.set("UPDATE urunResim SET guncelleyenKullaniciID = " + Session["kullaniciID"] + ", guncellemeTarihi = GETDATE(), sira = " + resimSira[i] + " WHERE urunResimID = " + resimID[i]);
                    resim_ids += resimID[i] + ",";
                }
                SQL.set("UPDATE urunResim SET guncelleyenKullaniciID = " + Session["kullaniciID"] + ", guncellemeTarihi = GETDATE(), silindi = 1 WHERE urunID = " + urunID + " AND urunResimID NOT IN (" + resim_ids + "0" + ")");
                for (int i = 0; i < resimID.Length; i++)
                {
                    if (resimID[i] == 0)
                    {
                        if (resimPath[i] != null && resimPath[i].Length > 0)
                        {
                            result = string.Format(@"{0}", Guid.NewGuid());
                            byte[] imageBytes = Convert.FromBase64String(resimPath[i].Replace("data:image/jpeg;base64,", ""));
                            WebImage img = new WebImage(imageBytes);
                            var path = Path.Combine("~/Content/icerik/urun/orjinal", result);
                            img.Save(path);
                            img.Resize(1600, 1600, true, false).Crop(1, 1, 1, 1);
                            path = Path.Combine("~/Content/icerik/urun/buyuk", result);
                            img.Save(path);
                            img.Resize(400, 400, true, false).Crop(1, 1, 1, 1);
                            path = Path.Combine("~/Content/icerik/urun/kucuk", result);
                            img.Save(path);
                            result += Path.GetExtension(img.FileName);
                            result = result.Replace("jpg", "jpeg");
                            SQL.set("INSERT INTO urunResim (kaydedenKullaniciID, urunID, resim, sira) VALUES (" + Session["kullaniciID"] + ", " + urunID + ", '" + result + "', " + resimSira[i] + ")");
                        }
                    }
                }
            }
            else
            {
                SQL.set("UPDATE urunResim SET guncelleyenKullaniciID = " + Session["kullaniciID"] + ", guncellemeTarihi = GETDATE(), silindi = 1 WHERE urunID = " + urunID);
            }

            return RedirectToAction("Urun", new { tepki = 2 });
        }

        public ActionResult urunSil(int urunID)
        {
            SQL.set("UPDATE urunler SET guncelleyenKullaniciID = " + Session["kullaniciID"] + ", guncellemeTarihi = GETDATE(), silindi = 1 WHERE urunID = " + urunID);
            return RedirectToAction("Urun", new { tepki = 3 });
        }

        public ActionResult UrunDetay(int id = 0, int turet = 0)
        {
            if (Session["kullaniciID"] == null)
                return RedirectToAction("Login");

            ViewBag.urunID = id;
            ViewBag.turet = turet;

            return View();
        }
        #endregion
        #region Model Grubu
        public ActionResult ModelGrubu()
        {
            if (Session["kullaniciID"] == null)
                return RedirectToAction("Login");
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult modelGrubuEkle(string modelGrubu)
        {
            if (modelGrubu.Length <= 0)
                return RedirectToAction("ModelGrubu", new { hata = "Eksik bilgi girdiniz!" });

            SQL.set("INSERT INTO modelGrubu (kaydedenKullaniciID, modelGrubu) VALUES (" + Session["kullaniciID"] + ", '" + modelGrubu + "')");

            return RedirectToAction("ModelGrubu", new { tepki = 1 });
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult modelGrubuDuzenle(int modelGrubuID, string modelGrubu)
        {
            if (modelGrubu.Length <= 0)
                return RedirectToAction("ModelGrubu", new { hata = "Eksik bilgi girdiniz!" });

            SQL.set("UPDATE modelGrubu SET guncelleyenKullaniciID = " + Session["kullaniciID"] + ", guncellemeTarihi = GETDATE(), modelGrubu = '" + modelGrubu + "' WHERE modelGrubuID = " + modelGrubuID);

            return RedirectToAction("ModelGrubu", new { tepki = 1 });
        }

        public ActionResult modelGrubuSil(int modelGrubuID)
        {
            DataTable dt = SQL.get("SELECT * FROM urunler WHERE silindi = 0 AND modelGrubuID = " + modelGrubuID);
            if(dt.Rows.Count > 0)
                return RedirectToAction("ModelGrubu", new { hata = "Model grubuna dahil ürünler bulunmakta model grubu silinemez" });

            SQL.set("UPDATE modelGrubu SET guncelleyenKullaniciID = " + Session["kullaniciID"] + ", guncellemeTarihi = GETDATE(), silindi = 1 WHERE modelGrubuID = " + modelGrubuID);
            return RedirectToAction("ModelGrubu", new { tepki = 3 });
        }
        #endregion
        #region Marka
        public ActionResult Marka()
        {
            if (Session["kullaniciID"] == null)
                return RedirectToAction("Login");
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult markaEkle(string marka)
        {
            if (marka.Length <= 0)
                return RedirectToAction("Marka", new { hata = "Eksik bilgi girdiniz!" });

            SQL.set("INSERT INTO markalar (kaydedenKullaniciID, marka) VALUES (" + Session["kullaniciID"] + ", '" + marka + "')");

            return RedirectToAction("Marka", new { tepki = 1 });
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult markaDuzenle(int markaID, string marka)
        {
            if (marka.Length <= 0)
                return RedirectToAction("Marka", new { hata = "Eksik bilgi girdiniz!" });

            SQL.set("UPDATE markalar SET guncelleyenKullaniciID = " + Session["kullaniciID"] + ", guncellemeTarihi = GETDATE(), marka = '" + marka + "' WHERE markaID = " + markaID);

            return RedirectToAction("Marka", new { tepki = 1 });
        }

        public ActionResult markaSil(int markaID)
        {
            DataTable dt = SQL.get("SELECT * FROM urunler WHERE silindi = 0 AND markaID = " + markaID);
            if (dt.Rows.Count > 0)
                return RedirectToAction("Marka", new { hata = "Markaya dahil ürünler bulunmakta model grubu silinemez" });

            SQL.set("UPDATE markalar SET guncelleyenKullaniciID = " + Session["kullaniciID"] + ", guncellemeTarihi = GETDATE(), silindi = 1 WHERE markaID = " + markaID);
            return RedirectToAction("Marka", new { tepki = 3 });
        }
        #endregion
        #region Siparis
        public ActionResult Siparis(int id = 0)
        {
            if (Session["kullaniciID"] == null)
                return RedirectToAction("Login");

            SiparisFilter siparisFilter;
            if (Session["AdminSiparisFilter"] == null)
            {
                siparisFilter = new SiparisFilter(new List<int>(), new List<int>(), new List<int>(), new List<int>(), (id <= 0 ? 1 : id), 20);
            }
            else
            {
                siparisFilter = (SiparisFilter)Session["AdminSiparisFilter"];
                siparisFilter.page = (id <= 0 ? siparisFilter.page : id);
                siparisFilter.count = 20;
            }
            Session["AdminSiparisFilter"] = siparisFilter;

            ViewBag.siparisFilter = siparisFilter;

            return View();
        }
        public ActionResult SiparisDetay(int id)
        {
            if (Session["kullaniciID"] == null)
                return RedirectToAction("Login");

            Siparis s = new Siparis();
            s.Load(id);
            ViewBag.siparis = s;

            return View();
        }

        public ActionResult siparisDurumGuncelle(int siparisID, int siparisDurum)
        {
            if (Session["kullaniciID"] == null)
                return RedirectToAction("Login");

            Siparis s = new Siparis();
            s.Load(siparisID);
            s.DurumGuncelle(siparisDurum, Convert.ToInt32(Session["kullaniciID"]));
            return RedirectToAction("SiparisDetay", new { id = siparisID, hata = "Sipariş durumu güncellendi!" });
        }

        public ActionResult siparisKargoGuncelle(int siparisID, string kargoNo)
        {
            if (Session["kullaniciID"] == null)
                return RedirectToAction("Login");

            Siparis s = new Siparis();
            s.Load(siparisID);
            s.KargoNoGuncelle(kargoNo, Convert.ToInt32(Session["kullaniciID"]));
            return RedirectToAction("SiparisDetay", new { id = siparisID, hata = "Kargo no güncellendi!" });
        }
        #endregion
    }
}