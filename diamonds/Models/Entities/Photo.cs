using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Diamonds.Models;

namespace Diamonds.Models.Entities
{
    public partial class Photo
    {
        public int id { get; set; }
        public int galleryId { get; set; }
        public int userId { get; set; }
        public string descriptionPl { get; set; }
        public string descriptionEn { get; set; }
        public short no { get; set; }

        public string description { get { return lang(descriptionPl, descriptionEn); } }

        public virtual Gallery Gallery { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<Gallery> Galleries { get; set; }
        
        public Photo(HttpPostedFileBase fileJpg)
        {
            saveFileJpg(fileJpg);
        }

        public string path
        {
            get
            {
                return Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data/Galleries"), Gallery.id.ToString());
            }
        }

        public string jpgPath
        {
            get
            {
                return Path.Combine(path, id.ToString() + ".jpg");
            }
        }

        public void saveFileJpg(HttpPostedFileBase file)
        {
            if (file != null)
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                file.SaveAs(Path.Combine(path, id.ToString() + "_o.jpg"));
            }
        }

        public bool hasFileJpg
        {
            get
            {
                return File.Exists(jpgPath);
            }
        }

        public void deleteFileJpg()
        {
            if (hasFileJpg)
            {
                FileInfo file = new FileInfo(jpgPath);
                file.Delete();
            }
        }

        private string lang(string pl, string en)
        {
            if (HttpContext.Current.Response.Cookies["lang"].Value == "en" && en != "" || pl == "")
                return en;
            return pl;
        }
        
    }

    public class PhotoMapping : EntityTypeConfiguration<Photo>
    {
        public PhotoMapping()
            : base()
        {
            this.HasRequired(e => e.Gallery).WithMany(e => e.Photos).HasForeignKey(e => e.galleryId);
            this.HasRequired(e => e.User).WithMany(e => e.Photos).HasForeignKey(e => e.userId);

            this.HasMany(e => e.News).WithOptional(e => e.Photo).HasForeignKey(e => e.photoId);
            this.HasMany(e => e.Galleries).WithOptional(e => e.Cover).HasForeignKey(e => e.cover);
        }
    }
}