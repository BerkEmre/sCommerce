using sCommerce.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace sCommerce.Models
{
    public class Kullanici
    {
        public int kullaniciID;
        public DateTime kayitTarihi;
        public int kaydedenKullaniciID;
        public DateTime guncellemeTarihi;
        public int guncelleyenKullaniciID;
        public int silindi;
        public string ad;
        public string soyad;
        public string sifre;
        public string eMail;
        public int kullaniciTipiParametreID;
        public bool isValid = false;
        public List<Adres> adresler;
        public List<Siparis> siparisler;

        public Kullanici()
        {

        }

        public Kullanici(int kullaniciID, DateTime kayitTarihi, int kaydedenKullaniciID, DateTime guncellemeTarihi, int guncelleyenKullaniciID, int silindi, string ad, string soyad, string sifre, string eMail, int kullaniciTipiParametreID, bool isValid)
        {
            this.kullaniciID = kullaniciID;
            this.kayitTarihi = kayitTarihi;
            this.kaydedenKullaniciID = kaydedenKullaniciID;
            this.guncellemeTarihi = guncellemeTarihi;
            this.guncelleyenKullaniciID = guncelleyenKullaniciID;
            this.silindi = silindi;
            this.ad = ad;
            this.soyad = soyad;
            this.sifre = sifre;
            this.eMail = eMail;
            this.kullaniciTipiParametreID = kullaniciTipiParametreID;
            this.isValid = isValid;
        }

        public bool load(int kullaniciID)
        {
            DataTable dt = SQL.get("SELECT * FROM kullanicilar WHERE kullaniciID = " + kullaniciID);
            if(dt.Rows.Count <= 0)
            {
                isValid = false;
                return false;
            }

            DataRow dr = dt.Rows[0];
            this.kullaniciID = Convert.ToInt32(dr["kullaniciID"]);
            this.kayitTarihi = Convert.ToDateTime(dr["kayitTarihi"]);
            this.kaydedenKullaniciID = Convert.ToInt32(dr["kaydedenKullaniciID"]);
            DateTime.TryParse(dr["guncellemeTarihi"].ToString(), out this.guncellemeTarihi);
            Int32.TryParse(dr["guncelleyenKullaniciID"].ToString(), out this.guncelleyenKullaniciID);
            this.silindi = Convert.ToInt32(dr["silindi"]);
            this.ad = Convert.ToString(dr["ad"]);
            this.soyad = Convert.ToString(dr["soyad"]);
            this.sifre = Convert.ToString(dr["sifre"]);
            this.eMail = Convert.ToString(dr["eMail"]);
            this.kullaniciTipiParametreID = Convert.ToInt32(dr["kullaniciTipiParametreID"]);
            this.isValid = true;
            this.adresler = new Adres().GetKullaniciAdres(this.kullaniciID);

            return true;
        }

        public string adSoyad()
        {
            return ad + " " + soyad;
        }
    
        public string Kaydet(string ad, string soyad, string sifre, string eMail, int kullaniciTipiParametreID = 2)
        {
            DataRow dr = SQL.get("SELECT cnt = COUNT(*) FROM kullanicilar WHERE silindi = 0 AND eMail = '" + eMail + "'").Rows[0];
            if(Convert.ToInt32(dr["cnt"]) > 0)
            {
                return "'" + eMail + "' mail adresi kullanılmaktadır, lütfen farklı bir mail adresi giriniz.";
            }

            sifre = SQL.MD5(sifre);

            dr = SQL.get("INSERT INTO kullanicilar (kaydedenKullaniciID, ad, soyad, sifre, eMail, kullaniciTipiParametreID, telefon) VALUES(0, '" + ad + "', '" + soyad + "', '" + sifre + "', '" + eMail + "', " + kullaniciTipiParametreID + ", ''); SELECT SCOPE_IDENTITY();").Rows[0];
            int yeniKullaniciID = Convert.ToInt32(dr[0]);

            load(yeniKullaniciID);
            return "";
        }

        public string GirisKontrol(string eMail, string sifre, bool md5 = true)
        {
            if(md5)
                sifre = SQL.MD5(sifre);

            DataTable dt = SQL.get("SELECT * FROM kullanicilar WHERE silindi = 0 AND eMail = '" + eMail + "' AND sifre = '" + sifre + "'");
            if(dt.Rows.Count <= 0)
            {
                return "Mail adresiniz veya şifreniz hatalı...";
            }

            DataRow dr = dt.Rows[0];
            this.kullaniciID = Convert.ToInt32(dr["kullaniciID"]);
            this.kayitTarihi = Convert.ToDateTime(dr["kayitTarihi"]);
            this.kaydedenKullaniciID = Convert.ToInt32(dr["kaydedenKullaniciID"]);
            DateTime.TryParse(dr["guncellemeTarihi"].ToString(), out this.guncellemeTarihi);
            Int32.TryParse(dr["guncelleyenKullaniciID"].ToString(), out this.guncelleyenKullaniciID);
            this.silindi = Convert.ToInt32(dr["silindi"]);
            this.ad = Convert.ToString(dr["ad"]);
            this.soyad = Convert.ToString(dr["soyad"]);
            this.sifre = Convert.ToString(dr["sifre"]);
            this.eMail = Convert.ToString(dr["eMail"]);
            this.kullaniciTipiParametreID = Convert.ToInt32(dr["kullaniciTipiParametreID"]);
            this.isValid = true;
            this.adresler = new Adres().GetKullaniciAdres(this.kullaniciID);

            return "";
        }
    
        public string SifirlamaKey(string eMail)
        {
            DataTable dt = SQL.get("SELECT * FROM kullanicilar WHERE silindi = 0 AND eMail = '" + eMail + "'");
            if (dt.Rows.Count <= 0)
                return "";

            string siteUrl = string.Format("{0}://{1}{2}{3}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port == 80 ? string.Empty : ":" + HttpContext.Current.Request.Url.Port, HttpContext.Current.Request.ApplicationPath);

            Kripto kripto = new Kripto();
            

            DataRow dr = dt.Rows[0];
            string url = siteUrl + "/Home/RePass?a=" + kripto.DESSifrele(eMail) + "&b=" + kripto.DESSifrele(dr["sifre"].ToString());
            string icerik =
                "<table style='width:100%!important;height:100%!important;line-height:100%!important;border-spacing:0;border-collapse:collapse;font-size:16px;margin:0;padding:0;border:0' cellspacing='0' cellpadding='0' border='0' align='center'>" +
                "   <tbody>" +
                "       <tr>" +
                "           <td valign='top' bgcolor='#FFFFFF' align='center'>" +
                "               <table width='720' style='border-spacing:0;border-collapse:collapse;width:100%;max-width:45em;margin-top:2em;margin-bottom:2em;border-color:#e0e0e0 #dddddd #dddddd;border-style:solid;border-width:0.0625em' cellspacing='0' cellpadding='0' border='0' bgcolor='#fff'>" +
                "                   <tbody>" +
                "                       <tr>" +
                "                           <td bgcolor='#ffffff' style='padding:0 5%'>" +
                "                               <table style='border-spacing:0;border-collapse:collapse;width:100%' cellspacing='0' cellpadding='0' border='0'>" +
                "                                   <tbody>" +
                "                                       <tr>" +
                "                                           <td align='left' style='width:9.125em;padding-top:2em;padding-bottom:1em'>" +
                "                                               <a href='" + siteUrl + "' target='_blank'>" +
                "                                                   <img src='" + siteUrl + "/Content/icerik/site_logo/" + Site.siteBilgileri.logo + "' style='outline:none;text-decoration:none;display:block' border='0'>" +
                "                                               </a>" +
                "                                           </td>" +
                "                                           <td align='center' valign='middle' style='padding-top:1em;padding-left:1em;padding-right:1em;font-size:16px'>" +
                "                                               <table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                "                                                   <tbody>" +
                "                                                        <tr>" +
                "                                                            <td width='100%' height='1' style='width:100%;height:1px;font-size:0;border-bottom-width:1px;border-bottom-color:#e5e5e5;border-bottom-style:solid;line-height:0'> &nbsp;</td>" +
                "                                                       </tr>" +
                "                                                   </tbody>" +
                "                                               </table>" +
                "                                           </td>" +
                "                                       </tr>" +
                "                                   </tbody>" +
                "                               </table>" +
                "                           </td>" +
                "                       </tr>" +
                "                       <tr>" +
                "                           <td bgcolor='#ffffff' style='padding:1em 5% 0'>" +
                "                               <table width='' border='0' cellspacing='0' cellpadding='0'>" +
                "                                   <tbody>" +
                "                                       <tr>" +
                "                                           <td style='font-family:Helvetica,Arial,sans-serif;font-size:15px;line-height:23px;color:#484848;max-width:656px;padding:0.2em 0 0'>" +
                "                                               Merhaba  <strong>" + dr["ad"] + " " + dr["soyad"] + "</strong>,<br><br>" +
                "                                               Şifrenizi unuttuysanız üzülmeyin, aşağıdaki butona basarak yeni şifre oluşturabilirsiniz." +
                "                                           </td>" +
                "                                       </tr>" +
                "                                       <tr>" +
                "                                           <td align='center' style='padding:2.5em 0 0'>" +
                "                                               <table width='100%' cellspacing='0' cellpadding='0' border='0'>" +
                "                                                   <tbody>" +
                "                                                       <tr>" +
                "                                                           <td align='left'>" +
                "                                                               <a href='" + url + "' style='background-color:rgba(40, 121, 254, 0.9);border-radius:4px;color:#ffffff;display:inline-block;font-family:'AvenirNext',Arial,Helvetica,sans-serif;font-size:15px;font-weight:bold;line-height:44px;text-align:center;text-decoration:none;width:224px' target='_blank'>Yeni Şifre Oluştur</a>" +
                "                                                           </td>" +
                "                                                       </tr>" +
                "                                                   </tbody>" +
                "                                               </table>" +
                "                                           </td>" +
                "                                       </tr>" +
                "                                   </tbody> " +
                "                               </table>" +
                "                           </td>" +
                "                       </tr>" +
                "                       <tr>" +
                "                           <td bgcolor='#ffffff' style='padding:3em 5% 0'>" +
                "                               <table cellspacing='0' cellpadding='0' border='0'>" +
                "                                   <tbody>" +
                "                                       <tr>" +
                "                                           <td width='24' align='left' valign='top'></td>" +
                "                                           <td valign='top' style='font-family:Helvetica,Arial,sans-serif;font-size:13px;color:#484848;line-height:17px;padding-top:1px'>" +
                "                                               <span style='color:rgba(40, 121, 254, 0.9);font-weight:bold'>Önemli Hatırlatma:</span><br><br>Eğer şifre yenileme talebinin size ait olmadığını düşünüyorsanız lütfen bu e-postayı dikkate almayın." +
                "                                               Mevcut şifreniz ile giriş yapmaya devam edebilirsiniz." + 
                "                                           </td>" +
                "                                       </tr>" +
				"                                   </tbody>" +
                "                               </table>" +
                "                           </td>" +
                "                       </tr>" +
                "                       <tr>" +
                "                           <td bgcolor='#ffffff' style='padding:0 5% 1em'>" +
                "                               <table width='100%' cellspacing='0' cellpadding='0' border='0'>" +
                "                                   <tbody>" +
                "                                       <tr>" +
                "                                           <td align='left' style='font-family:Helvetica,Arial,sans-serif;font-size:15px;color:#484848;line-height:1.4'>" +
                "                                               <br><br>—<br><br>" +
                "                                               Teşekkür ederiz,<br>" +
                "                                               <strong>" + Site.siteBilgileri.siteAdi + "</strong>" +
                "                                           </td>" +
                "                                       </tr>" +
                "                                   </tbody>" +
                "                               </table>" +
                "                           </td>" +
                "                       </tr> " +
                "                   </tbody>" +
                "               </table>" +
                "           </td>" +
                "       </tr>" +
                "   </tbody>" +
                "</table>";

            Site.MailGonder("Şifre yenileme talebi", icerik, new List<string> { eMail });

            return "Mail Gönderildi";
        }

        public Kullanici SifirlamaCoz(string a, string b)
        {
            Kullanici k = new Kullanici();

            Kripto kripto = new Kripto();
            string eMail = kripto.DESCoz(a);
            string sifre = kripto.DESCoz(b);

            string hata = k.GirisKontrol(eMail, sifre, false);
            if (hata.Length > 1)
                return null;
            else
                return k;
        }
    
        public bool SifreSifirla(string eMail, string sifre)
        {
            SQL.set("UPDATE kullanicilar SET sifre = '" + SQL.MD5(sifre) + "' WHERE silindi = 0 AND eMail = '" + eMail + "'");

            return true;
        }
        
        public bool SifreGuncelle(string eMail, string sifre, string eskiSifre)
        {
            SQL.set("UPDATE kullanicilar SET sifre = '" + SQL.MD5(sifre) + "' WHERE silindi = 0 AND eMail = '" + eMail + "' AND sifre = '" + SQL.MD5(eskiSifre) + "'");

            return true;
        }
    
        public bool LoadSiparisler()
        {
            this.siparisler = new List<Siparis>();
            DataTable dt = SQL.get("SELECT TOP 20 * FROM siparisler WHERE silindi = 0 AND kullaniciID = " + this.kullaniciID + " ORDER by kayitTarihi DESC");
            foreach (DataRow dr in dt.Rows)
            {
                Siparis s = new Siparis();
                Int32.TryParse(dr["siparisID"].ToString(), out s.siparisID);
                DateTime.TryParse(dr["kayitTarihi"].ToString(), out s.kayitTarihi);
                Int32.TryParse(dr["kaydedenKullaniciID"].ToString(), out s.kaydedenKullaniciID);
                DateTime.TryParse(dr["guncellemeTarihi"].ToString(), out s.guncellemeTarihi);
                Int32.TryParse(dr["guncelleyenKullaniciID"].ToString(), out s.guncelleyenKullaniciID);
                Int32.TryParse(dr["silindi"].ToString(), out s.silindi);
                s.ad = dr["ad"].ToString();
                s.soyad = dr["soyad"].ToString();
                s.telefon = dr["telefon"].ToString();
                s.sehir = dr["sehir"].ToString();
                s.semt = dr["semt"].ToString();
                s.mahalle = dr["mahalle"].ToString();
                s.postaKodu = dr["postaKodu"].ToString();
                s.adresSatir1 = dr["adresSatir1"].ToString();
                s.adresSatir2 = dr["adresSatir2"].ToString();
                Int32.TryParse(dr["kullaniciID"].ToString(), out s.kullaniciID);
                Int32.TryParse(dr["siparisDurum"].ToString(), out s.siparisDurum);
                Int32.TryParse(dr["odemeTipi"].ToString(), out s.odemeTipi);
                s.siparisNotu = dr["siparisNotu"].ToString();
                s.kargoNo = dr["kargoNo"].ToString();
                Decimal.TryParse(dr["kargoUcreti"].ToString(), out s.kargoUcreti);
                s.siparisKalemleri = new SiparisKalem().GetSiparisKalem(s.siparisID);

                this.siparisler.Add(s);
            }

            return true;
        }
    }
}