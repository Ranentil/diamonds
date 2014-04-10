using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Diamonds.Models;

namespace Diamonds.Models.Entities
{
    public partial class Photo
    {
        public int id { get; set; }
        public int galleryId { get; set; }
        public short no { get; set; }
        public int userId { get; set; }
        public string descriptionPl { get; set; }
        public string descriptionEn { get; set; }

        public string description { get { return lang(descriptionPl, descriptionEn); } }

        public virtual Gallery Gallery { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<Gallery> Galleries { get; set; }
        public virtual ICollection<Player> Players { get; set; }

        public Photo() { }

        public Photo(int galleryId, int userId, HttpPostedFileBase fileJpg)
        {
            this.galleryId = galleryId;
            this.descriptionPl = "";
            this.descriptionEn = "";
            this.userId = userId;
        }

        public string path { get { return Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data/Galleries"), galleryId.ToString()); } }

        public string jpgPath { get { return Path.Combine(path, id.ToString() + ".jpg"); } }

        public string thumbPath { get { return Path.Combine(path, id.ToString() + "_thumb.jpg"); } }

        public string originalPath { get { return Path.Combine(path, id.ToString() + "_original.jpg"); } }

        public bool hasJpg { get { return File.Exists(jpgPath) || File.Exists(originalPath); } }

        public void saveJpg(HttpPostedFileBase file)
        {
            if (file != null)
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                file.SaveAs(originalPath);
                new ImageHandler(originalPath, thumbPath).SaveThumbnail();
                new ImageHandler(originalPath, jpgPath).SaveImage(1200, 1000);
            }
        }

        public void deleteFileJpg()
        {
            if (hasJpg)
            {
                FileInfo file = new FileInfo(jpgPath);
                file.Delete();
            }
        }

        private string lang(string pl, string en)
        {
            var cookie = HttpContext.Current.Request.Cookies["diamonds-lang"];
            if (cookie != null && cookie.Value == "en" || pl == "")
                if (en != "")
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
            this.HasMany(e => e.Galleries).WithOptional(e => e.Cover).HasForeignKey(e => e.coverId);
            this.HasMany(e => e.Players).WithOptional(e => e.Photo).HasForeignKey(e => e.photoId);
        }
    }
}