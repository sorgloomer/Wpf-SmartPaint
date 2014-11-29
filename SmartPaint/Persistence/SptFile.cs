using Ionic.Zip;
using SmartPaint.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace SmartPaint.Persistence
{
    public class SptFile
    {
        private Dictionary<BitmapSource, int> idByPicture;
        private Dictionary<int, BitmapImage> pictureById;
        private ZipFile zipFile;

        public Project Project { get; protected set; }

        public SptFile(Project project = null)
        {
            this.Project = project;
        }

        protected void PreprocessProjectSave()
        {
            var pictureCounter = 0;
            this.idByPicture = new Dictionary<BitmapSource, int>();
            foreach (var p in this.Project.Patches)
            {
                if (!this.idByPicture.ContainsKey(p.Image))
                {
                    this.idByPicture.Add(p.Image, pictureCounter++);
                }
            }
        }

        protected object PatchXml(Patch p)
        {
            return new XElement("Patch",
                new XElement("Name", p.Name),
                new XElement("Position",
                    new XElement("X", p.PositionX),
                    new XElement("Y", p.PositionY)),
                new XElement("ImageId", this.idByPicture[p.Image]));
        }
        protected XDocument CreateXml()
        {
            var patches = this.Project.Patches.Select(PatchXml).ToArray();
            return new XDocument(
                new XElement("Project",
                    new XElement("Patches", patches)));
        }

        protected void SaveMetaToZip()
        {
            var ms = new MemoryStream();
            this.CreateXml().Save(ms);
            this.zipFile.AddEntry("Meta.xml", ms.ToArray());
        }
        protected void SavePreprocessedImagesToZip()
        {
            foreach (var p in this.idByPicture)
            {
                var stream = new MemoryStream();
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(p.Key));
                encoder.Save(stream);
                this.zipFile.AddEntry("images/" + p.Value.ToString() + ".png", stream.ToArray());
            }
        }
        protected ZipFile MakeZipFile()
        {
            this.PreprocessProjectSave();
            var file = new ZipFile();
            this.zipFile = file;
            this.SaveMetaToZip();
            this.SavePreprocessedImagesToZip();
            this.idByPicture = null;
            this.zipFile = null;
            return file;
        }

        public void SaveTo(string path)
        {
            using (var file = this.MakeZipFile()) file.Save(path);
        }
        public void SaveTo(Stream stream)
        {
            using (var file = this.MakeZipFile()) file.Save(stream);
        }


        protected BitmapImage LoadImageFromZip(int id)
        {
            BitmapImage image = null;
            if (!this.pictureById.TryGetValue(id, out image))
            {
                image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = this.zipFile["images/" + id.ToString() + ".png"].InputStream;
                image.EndInit();
                this.pictureById.Add(id, image);
            }
            return image;
        }
        protected Patch MapElement(XElement element)
        {
            return new Patch(
                (string)element.Element("Name"),
                this.LoadImageFromZip((int)element.Element("ImageId")),
                (int)element.Element("Position").Element("X"),
                (int)element.Element("Position").Element("Y"));
        }
        public Project LoadFrom(ZipFile file)
        {
            this.zipFile = file;
            var metaEntry = file["Meta.xml"];
            var xml = XDocument.Load(metaEntry.InputStream);
            this.pictureById = new Dictionary<int, BitmapImage>();
            var patches = xml.Element("Project").Element("Patches").Elements("Patch").Select(this.MapElement);

            this.pictureById = null;
            this.idByPicture = null;
            this.zipFile = null;
            return this.Project = new Project() { 
                Patches = new ObservableCollection<Patch>(patches)
            };
        }

        public Project LoadFrom(Stream stream)
        {
            using (var file = ZipFile.Read(stream)) return this.LoadFrom(file);
        }
        public Project LoadFrom(string path)
        {
            using (var file = ZipFile.Read(path)) return this.LoadFrom(file);
        }

        public static Project From(string path)
        {
            var spt = new SptFile();
            return spt.LoadFrom(path);
        }
        public static void Save(string path, Project project)
        {
            new SptFile(project).SaveTo(path);
        }
    }
}


