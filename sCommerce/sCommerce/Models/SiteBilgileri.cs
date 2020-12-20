using sCommerce.Helper;
using System.Data;

namespace sCommerce.Models
{
    public class SiteBilgileri
    {
        public string siteAdi;
        public string tel;
        public string gsm;
        public string whatsapp;
        public string eMail;
        public string calismaSaatleri;
        public string adres;
        public string seoAciklama;
        public string seoKeywords;
        public string slogan;
        public string hakkimizda;
        public string fax;
        public string facebook;
        public string twitter;
        public string instagram;
        public string youtube;
        public string linkedin;
        public string pinterest;
        public string uyelikSozlesmesi;
        public string kullanimSartlari;
        public string mesafeliSatisSozlesmesi;
        public string gizlilikPolitikasi;
        public string sikSorulanSorular;
        public string logo;
        public string iban;
        public string banka;
        public string hesapNo;
        public string sube;

        public bool load()
        {
            DataRow dr = SQL.get("SELECT TOP 1 * FROM siteBilgileri").Rows[0];
            this.siteAdi = dr["siteAdi"].ToString();
            this.tel = dr["tel"].ToString();
            this.gsm = dr["gsm"].ToString();
            this.whatsapp = dr["whatsapp"].ToString();
            this.eMail = dr["eMail"].ToString();
            this.calismaSaatleri = dr["calismaSaatleri"].ToString();
            this.adres = dr["adres"].ToString();
            this.seoAciklama = dr["seoAciklama"].ToString();
            this.seoKeywords = dr["seoKeywords"].ToString();
            this.slogan = dr["slogan"].ToString();
            this.hakkimizda = dr["hakkimizda"].ToString();
            this.fax = dr["fax"].ToString();
            this.facebook = dr["facebook"].ToString();
            this.twitter = dr["twitter"].ToString();
            this.instagram = dr["instagram"].ToString();
            this.youtube = dr["youtube"].ToString();
            this.linkedin = dr["linkedin"].ToString();
            this.pinterest = dr["pinterest"].ToString();
            this.uyelikSozlesmesi = dr["uyelikSozlesmesi"].ToString();
            this.kullanimSartlari = dr["kullanimSartlari"].ToString();
            this.mesafeliSatisSozlesmesi = dr["mesafeliSatisSozlesmesi"].ToString();
            this.gizlilikPolitikasi = dr["gizlilikPolitikasi"].ToString();
            this.sikSorulanSorular = dr["sikSorulanSorular"].ToString();
            this.logo = dr["logo"].ToString();
            this.iban = dr["iban"].ToString();
            this.banka = dr["banka"].ToString();
            this.hesapNo = dr["hesapNo"].ToString();
            this.sube = dr["sube"].ToString();

            return true;
        }
    }
}