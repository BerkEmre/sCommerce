﻿using sCommerce.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace sCommerce.Models
{
    public class Urun
    {
        public int urunID;
        public DateTime kayitTarihi;
        public int kaydedenKullaniciID;
        public DateTime guncellemeTarihi;
        public int guncelleyenKullaniciID;
        public int silindi;
        public string urunAdi;
        public string urunAciklamasi;
        public string seoAciklama;
        public string seoKeywords;
        public int urunEtiketiParametreID;
        public string barkod;
        public string stokKodu;
        public string depoLokasyonu;
        public decimal eskiFiyat;
        public decimal fiyat;
        public int vergiParametreID;
        public int vergiDahilSatis;
        public int miktar;
        public int minimumMiktar;
        public int stokBitinceParametreID;
        public decimal agirlik;
        public int kargoSuresi;
        public int urunDurumuParametreID;
        public int oneCikanlar;
        public int modelGrubuID;
        public int markaID;
        public Parametre urunEtiketi;
        public Parametre vergi;
        public Parametre stokBitince;
        public Parametre urunDurumu;
        public ModelGrubu modelGrubu;
        public Marka marka;
        public List<Kategori> urunKategorileri;
        public List<UrunResim> urunResimleri;
        public List<UrunOzellik> urunOzellikleri;

        public EntegrasyonBilgi entegrasyonBilgi;

        public List<Urun> GetUrunFromUrunEtiketi(int urunEtiketiParametreID)
        {
            List<Urun> u = new List<Urun>();

            DataTable dt = SQL.get(
                "SELECT " +
                "    u.*, " +
                "    urunEtiketi = pUEtiket.deger, " +
                "    vergi = pVergi.deger, " +
                "    stokBitince = pSBitince.deger, " +
                "    urunDurumu = pUDurumu.deger, " +
                "    modelGrubu = mg.modelGrubu, " +
                "    marka = m.marka " +
                "FROM " +
                "    urunler u " +
                "    LEFT OUTER JOIN parametreler pUEtiket ON pUEtiket.parametreID = u.urunEtiketiParametreID " +
                "    LEFT OUTER JOIN parametreler pVergi ON pVergi.parametreID = u.vergiParametreID " +
                "    LEFT OUTER JOIN parametreler pSBitince ON pSBitince.parametreID = u.stokBitinceParametreID " +
                "    LEFT OUTER JOIN parametreler pUDurumu ON pUDurumu.parametreID = u.urunDurumuParametreID " +
                "    LEFT OUTER JOIN modelGrubu mg ON mg.modelGrubuID = u.modelGrubuID " +
                "    LEFT OUTER JOIN markalar m ON m.markaID = u.markaID " +
                "WHERE " +
                "    u.silindi = 0 " +
                "    AND u.oneCikanlar = 1 " +
                "    AND u.urunDurumuParametreID != 9 " +
                "    AND u.urunEtiketiParametreID = " + urunEtiketiParametreID);

            foreach (DataRow dataRow in dt.Rows)
            {
                Urun urun = new Urun();
                Int32.TryParse(dataRow["urunID"].ToString(), out urun.urunID);
                DateTime.TryParse(dataRow["kayitTarihi"].ToString(), out urun.kayitTarihi);
                Int32.TryParse(dataRow["kaydedenKullaniciID"].ToString(), out urun.kaydedenKullaniciID);
                DateTime.TryParse(dataRow["guncellemeTarihi"].ToString(), out urun.guncellemeTarihi);
                Int32.TryParse(dataRow["guncelleyenKullaniciID"].ToString(), out urun.guncelleyenKullaniciID);
                Int32.TryParse(dataRow["silindi"].ToString(), out urun.silindi);
                urun.urunAdi = dataRow["urunAdi"].ToString();
                urun.urunAciklamasi = dataRow["urunAciklamasi"].ToString();
                urun.seoAciklama = dataRow["seoAciklama"].ToString();
                urun.seoKeywords = dataRow["seoKeywords"].ToString();
                Int32.TryParse(dataRow["urunEtiketiParametreID"].ToString(), out urun.urunEtiketiParametreID);
                urun.barkod = dataRow["barkod"].ToString();
                urun.stokKodu = dataRow["stokKodu"].ToString();
                urun.depoLokasyonu = dataRow["depoLokasyonu"].ToString();
                Decimal.TryParse(dataRow["eskiFiyat"].ToString(), out urun.eskiFiyat);
                Decimal.TryParse(dataRow["fiyat"].ToString(), out urun.fiyat);
                Int32.TryParse(dataRow["vergiParametreID"].ToString(), out urun.vergiParametreID);
                Int32.TryParse(dataRow["vergiDahilSatis"].ToString(), out urun.vergiDahilSatis);
                Int32.TryParse(dataRow["miktar"].ToString(), out urun.miktar);
                Int32.TryParse(dataRow["minimumMiktar"].ToString(), out urun.minimumMiktar);
                Int32.TryParse(dataRow["stokBitinceParametreID"].ToString(), out urun.stokBitinceParametreID);
                Decimal.TryParse(dataRow["agirlik"].ToString(), out urun.agirlik);
                Int32.TryParse(dataRow["kargoSuresi"].ToString(), out urun.kargoSuresi);
                Int32.TryParse(dataRow["urunDurumuParametreID"].ToString(), out urun.urunDurumuParametreID);
                Int32.TryParse(dataRow["oneCikanlar"].ToString(), out urun.oneCikanlar);
                Int32.TryParse(dataRow["modelGrubuID"].ToString(), out urun.modelGrubuID);
                Int32.TryParse(dataRow["markaID"].ToString(), out urun.markaID);
                urun.urunEtiketi = new Parametre(urun.urunEtiketiParametreID, dataRow["urunEtiketi"].ToString());
                urun.vergi = new Parametre(urun.urunEtiketiParametreID, dataRow["vergi"].ToString());
                urun.stokBitince = new Parametre(urun.urunEtiketiParametreID, dataRow["stokBitince"].ToString());
                urun.urunDurumu = new Parametre(urun.urunEtiketiParametreID, dataRow["urunDurumu"].ToString());
                urun.modelGrubu = new ModelGrubu(urun.modelGrubuID, DateTime.Now, 0, DateTime.Now, 0, 0, dataRow["modelGrubu"].ToString());
                urun.modelGrubu = new ModelGrubu(urun.markaID, DateTime.Now, 0, DateTime.Now, 0, 0, dataRow["marka"].ToString());
                //urun.urunKategorileri = new Kategori().GetUrunKategorileri(urun.urunID);
                urun.urunResimleri = new UrunResim().GetUrunResimleri(urun.urunID);
                urun.urunOzellikleri = new UrunOzellik().GetUrunOzellik(urun.urunID);

                u.Add(urun);
            }
            return u;
        }

        public bool LoadFromID(int urunID, bool kategoriYukle = false)
        {
            DataTable dt = SQL.get(
                "SELECT " +
                "    u.*, " +
                "    urunEtiketi = pUEtiket.deger, " +
                "    vergi = pVergi.deger, " +
                "    stokBitince = pSBitince.deger, " +
                "    urunDurumu = pUDurumu.deger, " +
                "    modelGrubu = mg.modelGrubu, " +
                "    marka = m.marka " +
                "FROM " +
                "    urunler u " +
                "    LEFT OUTER JOIN parametreler pUEtiket ON pUEtiket.parametreID = u.urunEtiketiParametreID " +
                "    LEFT OUTER JOIN parametreler pVergi ON pVergi.parametreID = u.vergiParametreID " +
                "    LEFT OUTER JOIN parametreler pSBitince ON pSBitince.parametreID = u.stokBitinceParametreID " +
                "    LEFT OUTER JOIN parametreler pUDurumu ON pUDurumu.parametreID = u.urunDurumuParametreID " +
                "    LEFT OUTER JOIN modelGrubu mg ON mg.modelGrubuID = u.modelGrubuID " +
                "    LEFT OUTER JOIN markalar m ON m.markaID = u.markaID " +
                "WHERE " +
                "    u.urunID = " + urunID);

            if (dt.Rows.Count <= 0)
                return false;

            DataRow dataRow = dt.Rows[0];

            Int32.TryParse(dataRow["urunID"].ToString(), out this.urunID);
            DateTime.TryParse(dataRow["kayitTarihi"].ToString(), out this.kayitTarihi);
            Int32.TryParse(dataRow["kaydedenKullaniciID"].ToString(), out this.kaydedenKullaniciID);
            DateTime.TryParse(dataRow["guncellemeTarihi"].ToString(), out this.guncellemeTarihi);
            Int32.TryParse(dataRow["guncelleyenKullaniciID"].ToString(), out this.guncelleyenKullaniciID);
            Int32.TryParse(dataRow["silindi"].ToString(), out this.silindi);
            this.urunAdi = dataRow["urunAdi"].ToString();
            this.urunAciklamasi = dataRow["urunAciklamasi"].ToString();
            this.seoAciklama = dataRow["seoAciklama"].ToString();
            this.seoKeywords = dataRow["seoKeywords"].ToString();
            Int32.TryParse(dataRow["urunEtiketiParametreID"].ToString(), out this.urunEtiketiParametreID);
            this.barkod = dataRow["barkod"].ToString();
            this.stokKodu = dataRow["stokKodu"].ToString();
            this.depoLokasyonu = dataRow["depoLokasyonu"].ToString();
            Decimal.TryParse(dataRow["eskiFiyat"].ToString(), out this.eskiFiyat);
            Decimal.TryParse(dataRow["fiyat"].ToString(), out this.fiyat);
            Int32.TryParse(dataRow["vergiParametreID"].ToString(), out this.vergiParametreID);
            Int32.TryParse(dataRow["vergiDahilSatis"].ToString(), out this.vergiDahilSatis);
            Int32.TryParse(dataRow["miktar"].ToString(), out this.miktar);
            Int32.TryParse(dataRow["minimumMiktar"].ToString(), out this.minimumMiktar);
            Int32.TryParse(dataRow["stokBitinceParametreID"].ToString(), out this.stokBitinceParametreID);
            Decimal.TryParse(dataRow["agirlik"].ToString(), out this.agirlik);
            Int32.TryParse(dataRow["kargoSuresi"].ToString(), out this.kargoSuresi);
            Int32.TryParse(dataRow["urunDurumuParametreID"].ToString(), out this.urunDurumuParametreID);
            Int32.TryParse(dataRow["oneCikanlar"].ToString(), out this.oneCikanlar);
            Int32.TryParse(dataRow["modelGrubuID"].ToString(), out this.modelGrubuID);
            Int32.TryParse(dataRow["markaID"].ToString(), out this.markaID);
            this.urunEtiketi = new Parametre(this.urunEtiketiParametreID, dataRow["urunEtiketi"].ToString());
            this.vergi = new Parametre(this.urunEtiketiParametreID, dataRow["vergi"].ToString());
            this.stokBitince = new Parametre(this.urunEtiketiParametreID, dataRow["stokBitince"].ToString());
            this.urunDurumu = new Parametre(this.urunEtiketiParametreID, dataRow["urunDurumu"].ToString());
            this.modelGrubu = new ModelGrubu(this.modelGrubuID, DateTime.Now, 0, DateTime.Now, 0, 0, dataRow["modelGrubu"].ToString());
            this.marka = new Marka(this.markaID, DateTime.Now, 0, DateTime.Now, 0, 0, dataRow["marka"].ToString());
            if(kategoriYukle)
                this.urunKategorileri = new Kategori().GetUrunKategorileri(this.urunID);
            this.urunResimleri = new UrunResim().GetUrunResimleri(this.urunID);
            this.urunOzellikleri = new UrunOzellik().GetUrunOzellik(this.urunID);

            return true;
        }
    
        public List<Urun> UrunAra(string searchText)
        {
            searchText = searchText.Replace(' ', '%');

            List<Urun> uruns = new List<Urun>();

            DataTable dt = SQL.get(
               "SELECT TOP 36 " +
               "    ROW_NUMBER() OVER(PARTITION by u.modelGrubuID ORDER by u.urunID) AS uniqSira, " +
               "    u.*, " +
               "    urunEtiketi = pUEtiket.deger, " +
               "    vergi = pVergi.deger, " +
               "    stokBitince = pSBitince.deger, " +
               "    urunDurumu = pUDurumu.deger, " +
               "    modelGrubu = mg.modelGrubu, " +
               "    marka = m.marka " +
               "FROM " +
               "    urunler u " +
               "    LEFT OUTER JOIN parametreler pUEtiket ON pUEtiket.parametreID = u.urunEtiketiParametreID " +
               "    LEFT OUTER JOIN parametreler pVergi ON pVergi.parametreID = u.vergiParametreID " +
               "    LEFT OUTER JOIN parametreler pSBitince ON pSBitince.parametreID = u.stokBitinceParametreID " +
               "    LEFT OUTER JOIN parametreler pUDurumu ON pUDurumu.parametreID = u.urunDurumuParametreID " +
               "    LEFT OUTER JOIN modelGrubu mg ON mg.modelGrubuID = u.modelGrubuID " +
               "    LEFT OUTER JOIN markalar m ON m.markaID = u.markaID " +
               "WHERE " +
               "    u.silindi = 0 " +
               "    AND u.urunDurumuParametreID != 9 " +
               "    AND (u.urunAdi LIKE '%" + searchText + "%' OR u.urunAciklamasi LIKE '%" + searchText + "%' OR mg.modelGrubu LIKE '%" + searchText + "%' OR m.marka LIKE '%" + searchText + "%' OR u.seoAciklama LIKE '%" + searchText + "%' OR u.seoKeywords LIKE '%" + searchText + "%')" +
               " ORDER by uniqSira");

            foreach (DataRow dataRow in dt.Rows)
            {
                Urun urun = new Urun();
                Int32.TryParse(dataRow["urunID"].ToString(), out urun.urunID);
                DateTime.TryParse(dataRow["kayitTarihi"].ToString(), out urun.kayitTarihi);
                Int32.TryParse(dataRow["kaydedenKullaniciID"].ToString(), out urun.kaydedenKullaniciID);
                DateTime.TryParse(dataRow["guncellemeTarihi"].ToString(), out urun.guncellemeTarihi);
                Int32.TryParse(dataRow["guncelleyenKullaniciID"].ToString(), out urun.guncelleyenKullaniciID);
                Int32.TryParse(dataRow["silindi"].ToString(), out urun.silindi);
                urun.urunAdi = dataRow["urunAdi"].ToString();
                urun.urunAciklamasi = dataRow["urunAciklamasi"].ToString();
                urun.seoAciklama = dataRow["seoAciklama"].ToString();
                urun.seoKeywords = dataRow["seoKeywords"].ToString();
                Int32.TryParse(dataRow["urunEtiketiParametreID"].ToString(), out urun.urunEtiketiParametreID);
                urun.barkod = dataRow["barkod"].ToString();
                urun.stokKodu = dataRow["stokKodu"].ToString();
                urun.depoLokasyonu = dataRow["depoLokasyonu"].ToString();
                Decimal.TryParse(dataRow["eskiFiyat"].ToString(), out urun.eskiFiyat);
                Decimal.TryParse(dataRow["fiyat"].ToString(), out urun.fiyat);
                Int32.TryParse(dataRow["vergiParametreID"].ToString(), out urun.vergiParametreID);
                Int32.TryParse(dataRow["vergiDahilSatis"].ToString(), out urun.vergiDahilSatis);
                Int32.TryParse(dataRow["miktar"].ToString(), out urun.miktar);
                Int32.TryParse(dataRow["minimumMiktar"].ToString(), out urun.minimumMiktar);
                Int32.TryParse(dataRow["stokBitinceParametreID"].ToString(), out urun.stokBitinceParametreID);
                Decimal.TryParse(dataRow["agirlik"].ToString(), out urun.agirlik);
                Int32.TryParse(dataRow["kargoSuresi"].ToString(), out urun.kargoSuresi);
                Int32.TryParse(dataRow["urunDurumuParametreID"].ToString(), out urun.urunDurumuParametreID);
                Int32.TryParse(dataRow["oneCikanlar"].ToString(), out urun.oneCikanlar);
                Int32.TryParse(dataRow["modelGrubuID"].ToString(), out urun.modelGrubuID);
                Int32.TryParse(dataRow["markaID"].ToString(), out urun.markaID);
                urun.urunEtiketi = new Parametre(urun.urunEtiketiParametreID, dataRow["urunEtiketi"].ToString());
                urun.vergi = new Parametre(urun.urunEtiketiParametreID, dataRow["vergi"].ToString());
                urun.stokBitince = new Parametre(urun.urunEtiketiParametreID, dataRow["stokBitince"].ToString());
                urun.urunDurumu = new Parametre(urun.urunEtiketiParametreID, dataRow["urunDurumu"].ToString());
                urun.modelGrubu = new ModelGrubu(urun.modelGrubuID, DateTime.Now, 0, DateTime.Now, 0, 0, dataRow["modelGrubu"].ToString());
                urun.marka = new Marka(urun.markaID, DateTime.Now, 0, DateTime.Now, 0, 0, dataRow["marka"].ToString());
                //urun.urunKategorileri = new Kategori().GetUrunKategorileri(urun.urunID);
                urun.urunResimleri = new UrunResim().GetUrunResimleri(urun.urunID);
                urun.urunOzellikleri = new UrunOzellik().GetUrunOzellik(urun.urunID);

                uruns.Add(urun);
            }

            return uruns;
        }
    
        public int BarkodAra(string barkod)
        {
            int urunID = 0;

            DataTable dt = SQL.get("SELECT TOP 1 urunID FROM urunler WHERE silindi = 0 AND barkod = '" + barkod + "'");
            if (dt.Rows.Count <= 0)
                return urunID;

            urunID = Convert.ToInt32(dt.Rows[0]["urunID"]);
            return urunID;
        } 
    
        public bool LoadEntegrasyonBilgi()
        {
            DataTable dt = SQL.get("SELECT TOP 1 * FROM entegrasyonBilgileri WHERE silindi = 0 AND urunID = " + urunID);

            if (dt.Rows.Count <= 0)
                return false;

            DataRow dataRow = dt.Rows[0];

            this.entegrasyonBilgi = new EntegrasyonBilgi();
            Int32.TryParse(dataRow["entegrasyonBilgiID"].ToString(), out this.entegrasyonBilgi.entegrasyonBilgiID);
            DateTime.TryParse(dataRow["kayitTarihi"].ToString(), out this.entegrasyonBilgi.kayitTarihi);
            Int32.TryParse(dataRow["kaydedenKullaniciID"].ToString(), out this.entegrasyonBilgi.kaydedenKullaniciID);
            DateTime.TryParse(dataRow["guncellemeTarihi"].ToString(), out this.entegrasyonBilgi.guncellemeTarihi);
            Int32.TryParse(dataRow["guncelleyenKullaniciID"].ToString(), out this.entegrasyonBilgi.guncelleyenKullaniciID);
            Int32.TryParse(dataRow["silindi"].ToString(), out this.entegrasyonBilgi.silindi);
            Int32.TryParse(dataRow["urunID"].ToString(), out this.entegrasyonBilgi.urunID);
            Int64.TryParse(dataRow["n11UrunID"].ToString(), out this.entegrasyonBilgi.n11UrunID);
            Int64.TryParse(dataRow["n11KategoriID"].ToString(), out this.entegrasyonBilgi.n11KategoriID);
            this.entegrasyonBilgi.n11KategoriAdi = dataRow["n11KategoriAdi"].ToString();
            Decimal.TryParse(dataRow["n11FiyatYuzde"].ToString(), out this.entegrasyonBilgi.n11FiyatYuzde);
            Decimal.TryParse(dataRow["n11FiyatArti"].ToString(), out this.entegrasyonBilgi.n11FiyatArti);
            this.entegrasyonBilgi.n11TemplateName = dataRow["n11TemplateName"].ToString();

            return true;
        }
    }

    public class UrunFilter
    {
        public string urunAdi;
        public List<int> kategoriIDs;
        public List<int> urunEtiketParametreIDs;
        public List<int> modelGrubuIDs;
        public List<int> markaIDs;
        public filtremeleTipleri ft;
        public bool oneCikanlar;

        public bool stoguBitenler = false;

        public int page;
        public int count;

        public UrunFilter(string urunAdi, List<int> kategoriIDs, List<int> urunEtiketParametreIDs, List<int> modelGrubuIDs, List<int> markaIDs, filtremeleTipleri ft, bool oneCikanlar, int page, int count)
        {
            this.urunAdi = urunAdi;
            this.kategoriIDs = kategoriIDs;
            this.urunEtiketParametreIDs = urunEtiketParametreIDs;
            this.modelGrubuIDs = modelGrubuIDs;
            this.markaIDs = markaIDs;
            this.page = page;
            this.count = count;
            this.ft = ft;
            this.oneCikanlar = oneCikanlar;
        }

        public List<Urun> GetUruns()
        {
            List<Urun> uruns = new List<Urun>();
            string orderBy = "urunID";

            switch (ft)
            {
                case filtremeleTipleri.onerilenSiralama: 
                    orderBy = "uniqSira"; 
                    break;
                case filtremeleTipleri.dusukFiyat: 
                    orderBy = "fiyat"; 
                    break;
                case filtremeleTipleri.yuksekFiyat: 
                    orderBy = "fiyat DESC"; 
                    break;
                case filtremeleTipleri.urunAdi:
                    orderBy = "urunAdi";
                    break;
                case filtremeleTipleri.urunAdiTersten:
                    orderBy = "urunAdi DESC";
                    break;
                case filtremeleTipleri.urunID:
                    orderBy = "urunID";
                    break;
                case filtremeleTipleri.urunIDTersten:
                    orderBy = "urunID DESC";
                    break;
            }

            DataTable dt = SQL.get(
                "SELECT " +
                "    ROW_NUMBER() OVER(PARTITION by u.modelGrubuID ORDER by u.urunID) AS uniqSira, " +
                "    u.*, " +
                "    urunEtiketi = pUEtiket.deger, " +
                "    vergi = pVergi.deger, " +
                "    stokBitince = pSBitince.deger, " +
                "    urunDurumu = pUDurumu.deger, " +
                "    modelGrubu = mg.modelGrubu, " +
                "    marka = m.marka " +
                "FROM " +
                "    urunler u " +
                "    LEFT OUTER JOIN parametreler pUEtiket ON pUEtiket.parametreID = u.urunEtiketiParametreID " +
                "    LEFT OUTER JOIN parametreler pVergi ON pVergi.parametreID = u.vergiParametreID " +
                "    LEFT OUTER JOIN parametreler pSBitince ON pSBitince.parametreID = u.stokBitinceParametreID " +
                "    LEFT OUTER JOIN parametreler pUDurumu ON pUDurumu.parametreID = u.urunDurumuParametreID " +
                "    LEFT OUTER JOIN modelGrubu mg ON mg.modelGrubuID = u.modelGrubuID " +
                "    LEFT OUTER JOIN markalar m ON m.markaID = u.markaID " +
                "WHERE " +
                "    u.silindi = 0 " +
                "    AND u.urunDurumuParametreID != 9 " +
                (stoguBitenler ? " AND u.miktar <= u.minimumMiktar " : "") +
                (urunAdi.Length > 0 ? " AND u.urunAdi LIKE '%" + urunAdi + "%' " : "") +
                (oneCikanlar ? " AND u.oneCikanlar = 1 " : "") +
                (kategoriIDs.Count > 0 ? " AND EXISTS (SELECT urunID FROM urunKategori uk WHERE uk.silindi = 0 AND uk.urunID = u.urunID AND uk.kategoriID IN (" + string.Join(",", kategoriIDs) + ")) " : "") +
                (urunEtiketParametreIDs.Count > 0 ? " AND u.urunEtiketiParametreID IN (" + string.Join(",", urunEtiketParametreIDs) + ") " : "") +
                (modelGrubuIDs.Count > 0 ? " AND u.modelGrubuID IN (" + string.Join(",", modelGrubuIDs) + ") " : "") +
                (markaIDs.Count > 0 ? " AND u.markaID IN (" + string.Join(",", markaIDs) + ") " : "") +
                " ORDER by " + orderBy + " OFFSET " + ((page - 1) * count) + " ROWS FETCH NEXT " + count + " ROWS ONLY");

            foreach (DataRow dataRow in dt.Rows)
            {
                Urun urun = new Urun();
                Int32.TryParse(dataRow["urunID"].ToString(), out urun.urunID);
                DateTime.TryParse(dataRow["kayitTarihi"].ToString(), out urun.kayitTarihi);
                Int32.TryParse(dataRow["kaydedenKullaniciID"].ToString(), out urun.kaydedenKullaniciID);
                DateTime.TryParse(dataRow["guncellemeTarihi"].ToString(), out urun.guncellemeTarihi);
                Int32.TryParse(dataRow["guncelleyenKullaniciID"].ToString(), out urun.guncelleyenKullaniciID);
                Int32.TryParse(dataRow["silindi"].ToString(), out urun.silindi);
                urun.urunAdi = dataRow["urunAdi"].ToString();
                urun.urunAciklamasi = dataRow["urunAciklamasi"].ToString();
                urun.seoAciklama = dataRow["seoAciklama"].ToString();
                urun.seoKeywords = dataRow["seoKeywords"].ToString();
                Int32.TryParse(dataRow["urunEtiketiParametreID"].ToString(), out urun.urunEtiketiParametreID);
                urun.barkod = dataRow["barkod"].ToString();
                urun.stokKodu = dataRow["stokKodu"].ToString();
                urun.depoLokasyonu = dataRow["depoLokasyonu"].ToString();
                Decimal.TryParse(dataRow["eskiFiyat"].ToString(), out urun.eskiFiyat);
                Decimal.TryParse(dataRow["fiyat"].ToString(), out urun.fiyat);
                Int32.TryParse(dataRow["vergiParametreID"].ToString(), out urun.vergiParametreID);
                Int32.TryParse(dataRow["vergiDahilSatis"].ToString(), out urun.vergiDahilSatis);
                Int32.TryParse(dataRow["miktar"].ToString(), out urun.miktar);
                Int32.TryParse(dataRow["minimumMiktar"].ToString(), out urun.minimumMiktar);
                Int32.TryParse(dataRow["stokBitinceParametreID"].ToString(), out urun.stokBitinceParametreID);
                Decimal.TryParse(dataRow["agirlik"].ToString(), out urun.agirlik);
                Int32.TryParse(dataRow["kargoSuresi"].ToString(), out urun.kargoSuresi);
                Int32.TryParse(dataRow["urunDurumuParametreID"].ToString(), out urun.urunDurumuParametreID);
                Int32.TryParse(dataRow["oneCikanlar"].ToString(), out urun.oneCikanlar);
                Int32.TryParse(dataRow["modelGrubuID"].ToString(), out urun.modelGrubuID);
                Int32.TryParse(dataRow["markaID"].ToString(), out urun.markaID);
                urun.urunEtiketi = new Parametre(urun.urunEtiketiParametreID, dataRow["urunEtiketi"].ToString());
                urun.vergi = new Parametre(urun.urunEtiketiParametreID, dataRow["vergi"].ToString());
                urun.stokBitince = new Parametre(urun.urunEtiketiParametreID, dataRow["stokBitince"].ToString());
                urun.urunDurumu = new Parametre(urun.urunEtiketiParametreID, dataRow["urunDurumu"].ToString());
                urun.modelGrubu = new ModelGrubu(urun.modelGrubuID, DateTime.Now, 0, DateTime.Now, 0, 0, dataRow["modelGrubu"].ToString());
                urun.marka = new Marka(urun.markaID, DateTime.Now, 0, DateTime.Now, 0, 0, dataRow["marka"].ToString());
                //urun.urunKategorileri = new Kategori().GetUrunKategorileri(urun.urunID);
                urun.urunResimleri = new UrunResim().GetUrunResimleri(urun.urunID);
                urun.urunOzellikleri = new UrunOzellik().GetUrunOzellik(urun.urunID);

                uruns.Add(urun);
            }

            return uruns;
        }

        public int GetCount()
        {
            int count = 0;

            count = Convert.ToInt32(SQL.get(
                "SELECT " +
                "    count = COUNT(*) " +
                "FROM " +
                "    urunler u " +
                "    LEFT OUTER JOIN parametreler pUEtiket ON pUEtiket.parametreID = u.urunEtiketiParametreID " +
                "    LEFT OUTER JOIN parametreler pVergi ON pVergi.parametreID = u.vergiParametreID " +
                "    LEFT OUTER JOIN parametreler pSBitince ON pSBitince.parametreID = u.stokBitinceParametreID " +
                "    LEFT OUTER JOIN parametreler pUDurumu ON pUDurumu.parametreID = u.urunDurumuParametreID " +
                "    LEFT OUTER JOIN modelGrubu mg ON mg.modelGrubuID = u.modelGrubuID " +
                "    LEFT OUTER JOIN markalar m ON m.markaID = u.markaID " +
                "WHERE " +
                "    u.silindi = 0 " +
                "    AND u.urunDurumuParametreID != 9 " +
                (stoguBitenler ? " AND u.miktar <= u.minimumMiktar " : "") +
                (urunAdi.Length > 0 ? " AND u.urunAdi LIKE '%" + urunAdi + "%' " : "") +
                (oneCikanlar ? " AND u.oneCikanlar = 1 " : "") +
                (kategoriIDs.Count > 0 ? " AND EXISTS (SELECT urunID FROM urunKategori uk WHERE uk.silindi = 0 AND uk.urunID = u.urunID AND uk.kategoriID IN (" + string.Join(",", kategoriIDs) + ")) " : "") +
                (urunEtiketParametreIDs.Count > 0 ? " AND u.urunEtiketiParametreID IN (" + string.Join(",", urunEtiketParametreIDs) + ") " : "") +
                (modelGrubuIDs.Count > 0 ? " AND u.modelGrubuID IN (" + string.Join(",", modelGrubuIDs) + ") " : "") +
                (markaIDs.Count > 0 ? " AND u.markaID IN (" + string.Join(",", markaIDs) + ") " : "")).Rows[0]["count"]);



            return count;
        }
    }
}