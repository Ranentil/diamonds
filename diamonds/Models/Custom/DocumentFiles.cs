using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;

namespace Diamonds.Models
{
    public class DocumentFiles
    {
        public string dirPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Docs/"));

        public List<string> FilePaths
        { 
            get { return Directory.EnumerateFiles(dirPath).ToList(); } 
        }

        public List<Document> Documents
        {
            get
            {
                List<Document> docs = new List<Document>();
                foreach (var path in FilePaths)
                    docs.Add(new Document(path));
                return docs;
            }
        }
    }

    public class Document
    {
        public string dirPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Docs/"));
        public string path { get; set; }

        public Document(string path) { }
        public Document(HttpPostedFileBase file)
        {
            this.path = Path.Combine(dirPath, file.FileName);
            file.SaveAs(path);
        }

        public void Delete()
        {
            var file = new FileInfo(path);
            file.Delete();
        }

        public string extension
        {
            get { return Path.GetExtension(path).Substring(1, Path.GetExtension(path).Length - 1); }
        }

        public DateTime date
        {
            get { return new FileInfo(path).LastWriteTime; }
        }

        public string size
        {
            get
            {
                float s = new FileInfo(path).Length / 1024f;
                if (s / 1024f > 1)
                    return (s / 1024f).ToString("# ##0.##") + " MB";
                else
                    return s.ToString("# ##0.##") + " kB";
            }
        }
    }
}