using sCommerce.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace sCommerce.Models
{
    public class HediyeCeki
    {
        public int hediyeCekiID;
        public int kaydedenKullaniciID;
        public DateTime kayitTarihi;
        public int guncelleyenKullaniciID;
        public DateTime guncellemeTarihi;
        public int silindi;
        public string kod;
        public DateTime sonKullanmaTarihi;
        public bool kargoBedava;
        public int siparisIndirimi;
        public int limit;
        public bool tekKullanim;
        public int durum;
        public int kullaniciID;

        public Kullanici kullanici;

        public List<HediyeCeki> GetAll()
        {
            List<HediyeCeki> hediyeCekleri = new List<HediyeCeki>();

            DataTable dt = SQL.get("SELECT hc.*, k.ad, k.soyad FROM hediyeCekleri hc LEFT OUTER JOIN kullanicilar k ON k.kullaniciID = hc.kullaniciID WHERE hc.silindi = 0");
            foreach (DataRow dataRow in dt.Rows)
            {
                HediyeCeki hediyeCeki = new HediyeCeki();

                Int32.TryParse(dataRow["hediyeCekiID"].ToString(), out hediyeCeki.hediyeCekiID);
                Int32.TryParse(dataRow["kaydedenKullaniciID"].ToString(), out hediyeCeki.kaydedenKullaniciID);
                DateTime.TryParse(dataRow["kayitTarihi"].ToString(), out hediyeCeki.kayitTarihi);
                Int32.TryParse(dataRow["guncelleyenKullaniciID"].ToString(), out hediyeCeki.guncelleyenKullaniciID);
                DateTime.TryParse(dataRow["guncellemeTarihi"].ToString(), out hediyeCeki.guncellemeTarihi);
                Int32.TryParse(dataRow["silindi"].ToString(), out hediyeCeki.silindi);
                hediyeCeki.kod = dataRow["kod"].ToString();
                DateTime.TryParse(dataRow["sonKullanmaTarihi"].ToString(), out hediyeCeki.sonKullanmaTarihi);
                Boolean.TryParse(dataRow["kargoBedava"].ToString(), out hediyeCeki.kargoBedava);
                Int32.TryParse(dataRow["siparisIndirimi"].ToString(), out hediyeCeki.siparisIndirimi);
                Int32.TryParse(dataRow["limit"].ToString(), out hediyeCeki.limit);
                Boolean.TryParse(dataRow["tekKullanim"].ToString(), out hediyeCeki.tekKullanim);
                Int32.TryParse(dataRow["durum"].ToString(), out hediyeCeki.durum);
                Int32.TryParse(dataRow["kullaniciID"].ToString(), out hediyeCeki.kullaniciID);

                Kullanici musteri = new Kullanici();
                musteri.kullaniciID = hediyeCeki.kullaniciID;
                musteri.ad = dataRow["ad"].ToString();
                musteri.soyad = dataRow["soyad"].ToString();
                hediyeCeki.kullanici = kullanici;

                hediyeCekleri.Add(hediyeCeki);
            }

            return hediyeCekleri;
        }

        public string GetDurum()
        {
            string durumText = "";

            if (durum == (int)hediyeCekiDurum.aktif)
                durumText = "Aktif";
            else if (durum == (int)hediyeCekiDurum.pasif)
                durumText = "Pasif";

            return durumText;
        }
    }
}