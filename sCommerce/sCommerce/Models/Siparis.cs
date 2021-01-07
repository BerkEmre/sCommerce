using System;
using System.Collections.Generic;
using sCommerce.Helper;
using System.Web;
using System.Data;
using System.Linq;
using System.Configuration;

namespace sCommerce.Models
{
    public class Siparis
    {
        public int siparisID;
        public DateTime kayitTarihi;
        public int kaydedenKullaniciID;
        public DateTime guncellemeTarihi;
        public int guncelleyenKullaniciID;
        public int silindi;
        public string ad;
        public string soyad;
        public string telefon;
        public string sehir;
        public string semt;
        public string mahalle;
        public string postaKodu;
        public string adresSatir1;
        public string adresSatir2;
        public int kullaniciID;
        public int siparisDurum;
        public int odemeTipi;
        public string siparisNotu;
        public string kargoNo;
        public decimal kargoUcreti;
        public List<SiparisKalem> siparisKalemleri;

        public string GetSiparisDurum()
        {
            if (this.siparisDurum == Convert.ToInt32(Helper.siparisDurum.hazirlaniyor))
            {
                return "Hazırlanıyor";
            }
            else if (this.siparisDurum == Convert.ToInt32(Helper.siparisDurum.iptalEdildi))
            {
                return "İptal Edildi";
            }
            else if (this.siparisDurum == Convert.ToInt32(Helper.siparisDurum.kargoda))
            {
                return "Kargoda";
            }
            else if (this.siparisDurum == Convert.ToInt32(Helper.siparisDurum.odemeBasarisiz))
            {
                return "Ödeme Başarısız";
            }
            else if (this.siparisDurum == Convert.ToInt32(Helper.siparisDurum.odemeBekliyor))
            {
                return "Ödeme Bekliyor";
            }
            else if (this.siparisDurum == Convert.ToInt32(Helper.siparisDurum.teslimEdildi))
            {
                return "Teslim Edildi";
            }
            else
                return "---";
        }

        public string GetOdemeTipi()
        {
            if (this.odemeTipi == Convert.ToInt32(odemeTipleri.havale))
            {
                return "Havale";
            }
            else if (this.odemeTipi == Convert.ToInt32(odemeTipleri.kapidaOdeme))
            {
                return "Kapıda Ödeme";
            }
            else if (this.odemeTipi == Convert.ToInt32(odemeTipleri.krediBankaKarti))
            {
                return "Kredi/Banka Kartı";
            }
            else if (this.odemeTipi == Convert.ToInt32(odemeTipleri.magazadanTeslim))
            {
                return "Mağazadan Teslim";
            }
            else
                return "---";
        }

        public decimal GetToplamFiyat()
        {
            decimal toplam = siparisKalemleri.Sum(x => x.miktar * x.fiyat);
            return toplam + this.kargoUcreti;
        }

        public decimal GetToplamAdet()
        {
            return siparisKalemleri.Sum(x => x.miktar);
        }

        public bool Load(int siparisID)
        {
            DataTable dt = SQL.get("SELECT * FROM siparisler WHERE silindi = 0 AND siparisID = " + siparisID);
            if (dt.Rows.Count <= 0)
                return false;

            DataRow dr = dt.Rows[0];

            Int32.TryParse(dr["siparisID"].ToString(), out this.siparisID);
            DateTime.TryParse(dr["kayitTarihi"].ToString(), out this.kayitTarihi);
            Int32.TryParse(dr["kaydedenKullaniciID"].ToString(), out this.kaydedenKullaniciID);
            DateTime.TryParse(dr["guncellemeTarihi"].ToString(), out this.guncellemeTarihi);
            Int32.TryParse(dr["guncelleyenKullaniciID"].ToString(), out this.guncelleyenKullaniciID);
            Int32.TryParse(dr["silindi"].ToString(), out this.silindi);
            this.ad = dr["ad"].ToString();
            this.soyad = dr["soyad"].ToString();
            this.telefon = dr["telefon"].ToString();
            this.sehir = dr["sehir"].ToString();
            this.semt = dr["semt"].ToString();
            this.mahalle = dr["mahalle"].ToString();
            this.postaKodu = dr["postaKodu"].ToString();
            this.adresSatir1 = dr["adresSatir1"].ToString();
            this.adresSatir2 = dr["adresSatir2"].ToString();
            Int32.TryParse(dr["kullaniciID"].ToString(), out this.kullaniciID);
            Int32.TryParse(dr["siparisDurum"].ToString(), out this.siparisDurum);
            Int32.TryParse(dr["odemeTipi"].ToString(), out this.odemeTipi);
            this.siparisNotu = dr["siparisNotu"].ToString();
            this.kargoNo = dr["kargoNo"].ToString();
            Decimal.TryParse(dr["kargoUcreti"].ToString(), out this.kargoUcreti);
            this.siparisKalemleri = new SiparisKalem().GetSiparisKalem(this.siparisID);
            return true;
        }

        public bool DurumGuncelle(int siparisDurum, int kullaniciID)
        {
            SQL.set("UPDATE siparisler SET guncelleyenKullaniciID = " + kullaniciID + ", guncellemeTarihi = GETDATE(), siparisDurum = " + siparisDurum + " WHERE siparisID = " + this.siparisID);

            return true;
        }

        public bool KargoNoGuncelle(string kargoNo, int kullaniciID)
        {
            SQL.set("UPDATE siparisler SET guncelleyenKullaniciID = " + kullaniciID + ", guncellemeTarihi = GETDATE(), kargoNo = '" + kargoNo + "' WHERE siparisID = " + this.siparisID);
            return true;
        }

        public int Kaydet(int adresID, int odemeTipi, string siparisNotu)
        {
            Kullanici musteri = Site.GetMusteri();
            if (musteri == null)
                return 0;

            List<Sepet> sepet = Site.GetSepet(true);
            if (sepet.Count <= 0)
                return 0;

            Adres adres = new Adres();
            if(!adres.LoadFromID(adresID))
                return 0;

            int siparisDurum = 0;
            if (odemeTipi == (int)odemeTipleri.havale)
                siparisDurum = (int)Helper.siparisDurum.odemeBekliyor;
            else if (odemeTipi == (int)odemeTipleri.kapidaOdeme)
                siparisDurum = (int)Helper.siparisDurum.hazirlaniyor;
            else if (odemeTipi == (int)odemeTipleri.krediBankaKarti)
                siparisDurum = (int)Helper.siparisDurum.odemeBekliyor;
            else if (odemeTipi == (int)odemeTipleri.magazadanTeslim)
                siparisDurum = (int)Helper.siparisDurum.hazirlaniyor;

            decimal toplam = sepet.Sum(x => x.urun.fiyat * x.miktar);
            decimal kargoUcreti = 0;
            if (Convert.ToDecimal(ConfigurationManager.AppSettings["ucretsiz_kargo"]) > toplam)
            {
                kargoUcreti = Convert.ToDecimal(ConfigurationManager.AppSettings["kargo_ucreti"]);
            }


            string siparisID = SQL.get("INSERT INTO siparisler (kaydedenKullaniciID, ad, soyad, telefon, sehir, semt, mahalle, postaKodu, adresSatir1, adresSatir2, kullaniciID, siparisDurum, odemeTipi, siparisNotu, kargoNo, kargoUcreti) VALUES " +
                "(" + musteri.kullaniciID + ", '" + adres.ad + "', '" + adres.soyad + "', '" + adres.telefon + "', '" + adres.sehir + "', '" + adres.semt + "', '" + adres.mahalle + "', '" + adres.postaKodu + "', '" + adres.adresSatir1 + "', " + 
                "'" + adres.adresSatir2 + "', " + musteri.kullaniciID + ", " + siparisDurum + ", " + odemeTipi + ", '" + siparisNotu + "', '', " + kargoUcreti.ToString().Replace(',', '.') + "); SELECT SCOPE_IDENTITY();").Rows[0][0].ToString();

            foreach (Sepet sepetItem in sepet)
            {
                SQL.set("INSERT INTO siparisKalemleri (kaydedenKullaniciID, siparisID, urunID, miktar, fiyat) VALUES (" + musteri.kullaniciID + ", " + siparisID + ", " + sepetItem.urun.urunID + ", " + sepetItem.miktar + ", " + sepetItem.urun.fiyat.ToString().Replace(',', '.') + ")");
                SQL.set("UPDATE urunler SET miktar = miktar - " + sepetItem.miktar + " WHERE urunID = " + sepetItem.urun.urunID);//STOKLARINI DÜŞÜR
            }

            string siteUrl = string.Format("{0}://{1}{2}{3}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port == 80 ? string.Empty : ":" + HttpContext.Current.Request.Url.Port, HttpContext.Current.Request.ApplicationPath);
            HttpContext.Current.Session["sepet"] = null;
            Site.MailGonder("Yeni Sipariş", "<h2>Tebrikler!</h2><p>#" + siparisID + " sipariş no ile yeni bir siparişiniz var.</p><p><a href='" + siteUrl + "/Admin'>Siparişe Git</a></p>", new List<string>{ Site.siteBilgileri.eMail });
            SiparisMail(Convert.ToInt32(siparisID), musteri.adSoyad(), musteri.eMail);
            return Convert.ToInt32(siparisID);
        }
    
        public bool SiparisMail(int siparisID, string adSoyad, string eMail)
        {
            try
            {
                string siteUrl = string.Format("{0}://{1}{2}{3}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port == 80 ? string.Empty : ":" + HttpContext.Current.Request.Url.Port, HttpContext.Current.Request.ApplicationPath);
                string icerik =
                    "<table width='680' border='0' align='center' bgcolor='#ffffff'>" +
                    "   <tbody>" +
                    "		<tr>" +
                    "           <td style='width:95%;max-width:660px;border:1px solid #dddddd'>" +
                    "               <table width='95%' style='width:95%;max-width:640px;' align='center'>" +
                    "	                <tbody>" +
                    "    	                <tr>" +
                    "        	                <td style='border-bottom-color:#cccccc;border-bottom-style:solid;border-width:0 0 1px'>" +
                    "            	                <table width='100%'>" +
                    "                	                <tbody>" +
                    "                    	                <tr>" +
                    "                        	                <th>" +
                    "                            	                <table width='100%'>" +
                    "                                	                <tbody>" +
                    "                                    	                <tr>" +
                    "                                        	                <td>" +
                    "                                            	                <a href='" + siteUrl + "' target='_blank'>" +
                    "                                                	                <img src='" + siteUrl + "/Content/icerik/site_logo/" + Site.siteBilgileri.logo + "' width='200'>" +
                    "                                                               </a>" +
                    "                                                           </td>" +
                    "                                                       </tr>" +
                    "                                                   </tbody>" +
                    "                                               </table>" +
                    "                                           </th>" +
                    "                                       </tr>" +
                    "                                   </tbody>" +
                    "                               </table>" +
                    "                           </td>" +
                    "                       </tr>" +
                    "                   </tbody>" +
                    "               </table>" +
                    "               <table>" +
                    "                   <tbody>" +
                    "                       <tr>" +
                    "	                        <td width='602' height='40' style='font-family:Arial,Helvetica,sans-serif;font-size:16px;font-weight:bold;color:#484848;border-collapse:collapse;padding:20px 0 5px 10px;'>" +
                    "                               Merhaba " + adSoyad + ", siparişiniz oluşturuldu." +
                    "                           </td>" +
                    "                       </tr>" +
                    "                   </tbody>" +
                    "               </table>" +
                    "               <table width='100%' bgcolor='#f3f3f4'>" +
                    "                   <tbody>" +
                    "                       <tr>" +
                    "	                        <td align='left' style='font-family:Arial,Helvetica,sans-serif;font-size:16px;color:#484848;border-collapse:collapse;font-weight:normal;padding:5px 10px;border:0' valign='top'>" +
                    "                               Sipariş Numaranız: <font style='font-weight:bold;color:#ff6000;direction:ltr!important'>#" + siparisID + "</font>" +
                    "                           </td>" +
                    "                       </tr>" +
                    "                   </tbody>" +
                    "               </table>" +
                    "               <table>" +
                    "                   <tbody>" +
                    "                       <tr>" +
                    "                           <td style='font-family:Arial;font-size:13px;line-height:18px;color:#484848;border-collapse:collapse;font-weight:normal;padding:15px 10px;border:0'>" +
                    "                               Alışverişinizde bizi tercih ettiğiniz için teşekkür ederiz, siparişinizin durumunu hesabım sayfasından takip edebilirsiniz." +
                    "                           </td>" +
                    "                       </tr>" +
                    "                   </tbody>" +
                    "               </table>" +
                    "           </td>" +
                    "       </tr>" +
                    "   </tbody>" +
                    "</table>";

                Site.MailGonder("Şiparişinizi Aldık", icerik, new List<string> { eMail });
            }
            catch (Exception e)
            {

            }
            
            return true;
        }
    }

    public class SiparisFilter
    {
        public List<int> siparisIDs;
        public List<int> kullaniciIDs;
        public List<int> siparisDurums;
        public List<int> odemeTipis;

        public int page;
        public int count;

        public SiparisFilter(List<int> siparisIDs, List<int> kullaniciIDs, List<int> siparisDurums, List<int> odemeTipis, int page, int count)
        {
            this.siparisIDs = siparisIDs;
            this.kullaniciIDs = kullaniciIDs;
            this.siparisDurums = siparisDurums;
            this.odemeTipis = odemeTipis;
            this.page = page;
            this.count = count;
        }

        public List<Siparis> GetSiparisler()
        {
            List<Siparis> siparisler = new List<Siparis>();

            DataTable dt = SQL.get(
                "SELECT " +
                "   * " +
                "FROM " +
                "   siparisler " +
                "WHERE " +
                "   silindi = 0 " +
                (siparisIDs.Count > 0 ? " AND siparisID IN (" + string.Join(",", siparisIDs) + ") " : "") +
                (kullaniciIDs.Count > 0 ? " AND kullaniciID IN (" + string.Join(",", kullaniciIDs) + ") " : "") +
                (siparisDurums.Count > 0 ? " AND siparisDurum IN (" + string.Join(",", siparisDurums) + ") " : "") +
                (odemeTipis.Count > 0 ? " AND odemeTipi IN (" + string.Join(",", odemeTipis) + ") " : "") +
                " ORDER by siparisID DESC OFFSET " + ((page - 1) * count) + " ROWS FETCH NEXT " + count + " ROWS ONLY");
            foreach (DataRow dr in dt.Rows)
            {
                Siparis siparis = new Siparis();

                Int32.TryParse(dr["siparisID"].ToString(), out siparis.siparisID);
                DateTime.TryParse(dr["kayitTarihi"].ToString(), out siparis.kayitTarihi);
                Int32.TryParse(dr["kaydedenKullaniciID"].ToString(), out siparis.kaydedenKullaniciID);
                DateTime.TryParse(dr["guncellemeTarihi"].ToString(), out siparis.guncellemeTarihi);
                Int32.TryParse(dr["guncelleyenKullaniciID"].ToString(), out siparis.guncelleyenKullaniciID);
                Int32.TryParse(dr["silindi"].ToString(), out siparis.silindi);
                siparis.ad = dr["ad"].ToString();
                siparis.soyad = dr["soyad"].ToString();
                siparis.telefon = dr["telefon"].ToString();
                siparis.sehir = dr["sehir"].ToString();
                siparis.semt = dr["semt"].ToString();
                siparis.mahalle = dr["mahalle"].ToString();
                siparis.postaKodu = dr["postaKodu"].ToString();
                siparis.adresSatir1 = dr["adresSatir1"].ToString();
                siparis.adresSatir2 = dr["adresSatir2"].ToString();
                Int32.TryParse(dr["kullaniciID"].ToString(), out siparis.kullaniciID);
                Int32.TryParse(dr["siparisDurum"].ToString(), out siparis.siparisDurum);
                Int32.TryParse(dr["odemeTipi"].ToString(), out siparis.odemeTipi);
                siparis.siparisNotu = dr["siparisNotu"].ToString();
                siparis.kargoNo = dr["kargoNo"].ToString();
                Decimal.TryParse(dr["kargoUcreti"].ToString(), out siparis.kargoUcreti);
                siparis.siparisKalemleri = new SiparisKalem().GetSiparisKalem(siparis.siparisID);

                siparisler.Add(siparis);
            }

            return siparisler;
        }
    }
}