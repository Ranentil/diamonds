using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Data.Entity.ModelConfiguration;

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


        private string lang(string pl, string en)
        {
            if (HttpContext.Current.Response.Cookies["lang"].Value == "en" || pl == "")
                if (en != "")
                    return en;
            return pl;
        }

        public Photo(HttpPostedFileBase fileJpg, int galleryId)
        {
            this.galleryId = galleryId;
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
                file.SaveAs(jpgPath);
            }
        }

        public bool hasFileJpg()
        {
            return File.Exists(jpgPath);
        }

        public void deleteFileJpg()
        {
            if (hasFileJpg())
            {
                FileInfo file = new FileInfo(jpgPath);
                file.Delete();
            }
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
        }
    }
}