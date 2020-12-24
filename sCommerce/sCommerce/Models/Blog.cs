using sCommerce.Helper;
using System;
using System.Collections.Generic;
using System.Data;

namespace sCommerce.Models
{
    public class Blog
    {
        public int blogID;
        public DateTime kayitTarihi;
        public int kaydedenKullaniciID;
        public DateTime guncellemeTarihi;
        public int guncelleyenKullaniciID;
        public int silindi;
        public int kategoriID;
        public string gorsel;
        public string baslik;
        public string yazi;
        public Kullanici kaydedenKullanici;
        public Kategori kategori;

        public int BlogCount()
        {
            DataRow dr = SQL.get("SELECT cnt = COUNT(*) FROM bloglar WHERE silindi = 0").Rows[0];
            return Convert.ToInt32(dr["cnt"]);
        }

        public bool IsBlog(int blogID)
        {
            DataTable dt = SQL.get("SELECT * FROM bloglar WHERE silindi = 0 AND blogID = " + blogID);
            return dt.Rows.Count > 0;
        }

        public List<Blog> GetBlogs(int page, int count)
        {
            List<Blog> blogs = new List<Blog>();

            DataTable dt = SQL.get("SELECT b.*, k.ad, k.soyad, ka.kategori FROM bloglar b INNER JOIN kullanicilar k ON k.kullaniciID = b.kaydedenKullaniciID INNER JOIN kategoriler ka ON ka.kategoriID = b.kategoriID WHERE b.silindi = 0 ORDER by b.kayitTarihi DESC OFFSET " + ((page - 1) * count) + " ROWS FETCH NEXT " + count + " ROWS ONLY");
            foreach (DataRow dataRow in dt.Rows)
            {
                Blog blog = new Blog();

                Int32.TryParse(dataRow["blogID"].ToString(), out blog.blogID);
                DateTime.TryParse(dataRow["kayitTarihi"].ToString(), out blog.kayitTarihi);
                Int32.TryParse(dataRow["kaydedenKullaniciID"].ToString(), out blog.kaydedenKullaniciID);
                DateTime.TryParse(dataRow["guncellemeTarihi"].ToString(), out blog.guncellemeTarihi);
                Int32.TryParse(dataRow["guncelleyenKullaniciID"].ToString(), out blog.guncelleyenKullaniciID);
                Int32.TryParse(dataRow["silindi"].ToString(), out blog.silindi);
                Int32.TryParse(dataRow["kategoriID"].ToString(), out blog.kategoriID);
                blog.gorsel = dataRow["gorsel"].ToString();
                blog.baslik = dataRow["baslik"].ToString();
                blog.yazi = dataRow["yazi"].ToString();
                blog.kaydedenKullanici = new Kullanici(blog.kaydedenKullaniciID, DateTime.Now, 0, DateTime.Now, 0, 0, dataRow["ad"].ToString(), dataRow["soyad"].ToString(), "", "", 0, true);
                blog.kategori = new Kategori(blog.kategoriID, dataRow["kategori"].ToString());                

                blogs.Add(blog);
            }
            return blogs;
        }

        public void Load(int blogID)
        {
            DataTable dt = SQL.get("SELECT b.*, k.ad, k.soyad, ka.kategori FROM bloglar b INNER JOIN kullanicilar k ON k.kullaniciID = b.kaydedenKullaniciID INNER JOIN kategoriler ka ON ka.kategoriID = b.kategoriID WHERE b.silindi = 0 AND b.blogID = " + blogID);
            if(dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];

                Int32.TryParse(dataRow["blogID"].ToString(), out this.blogID);
                DateTime.TryParse(dataRow["kayitTarihi"].ToString(), out this.kayitTarihi);
                Int32.TryParse(dataRow["kaydedenKullaniciID"].ToString(), out this.kaydedenKullaniciID);
                DateTime.TryParse(dataRow["guncellemeTarihi"].ToString(), out this.guncellemeTarihi);
                Int32.TryParse(dataRow["guncelleyenKullaniciID"].ToString(), out this.guncelleyenKullaniciID);
                Int32.TryParse(dataRow["silindi"].ToString(), out this.silindi);
                Int32.TryParse(dataRow["kategoriID"].ToString(), out this.kategoriID);
                this.gorsel = dataRow["gorsel"].ToString();
                this.baslik = dataRow["baslik"].ToString();
                this.yazi = dataRow["yazi"].ToString();
                this.kaydedenKullanici = new Kullanici(this.kaydedenKullaniciID, DateTime.Now, 0, DateTime.Now, 0, 0, dataRow["ad"].ToString(), dataRow["soyad"].ToString(), "", "", 0, true);
                this.kategori = new Kategori(this.kategoriID, dataRow["kategori"].ToString());
            }
        }
    }
}