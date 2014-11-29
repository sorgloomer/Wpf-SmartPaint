using Ionic.Zip;
using SmartPaint.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace SmartPaint.Persistence
{
    public class SaveProject
    {
        private IDictionary<BitmapSource, int> pictures;
        public Project Project { get; protected set; }

        public SaveProject(Project project = null)
        {
            this.Project = project;
        }

        protected void PreprocessProject()
        {
            var pictureCounter = 0;
            this.pictures = new Dictionary<BitmapSource, int>();
            foreach (var p in this.Project.Patches)
            {
                if (!this.pictures.ContainsKey(p.Image))
                {
                    this.pictures.Add(p.Image, pictureCounter++);
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
                new XElement("ImageId", this.pictures[p.Image]));
        }
        protected XDocument CreateXml()
        {
            var patches = this.Project.Patches.Select(PatchXml).ToArray();
            return new XDocument(
                new XElement("Project",
                    new XElement("Patches", patches)));
        }

        protected ZipFile MakeZipFile()
        {
            this.PreprocessProject();
            var file = new ZipFile();
            file.AddEntry("meta.xml", (name, stream) => { this.CreateXml().Save(stream); });
            foreach (var p in this.pictures)
            {
                var stream = new MemoryStream();

                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(p.Key));
                encoder.Save(stream);

                var entry = file.AddEntry("images/" + p.Value.ToString() + ".png", stream.ToArray());
            }
            return file;
        }

        public void Save(string path)
        {
            using (var file = this.MakeZipFile()) file.Save(path);
        }
        public void Save(Stream stream)
        {
            using (var file = this.MakeZipFile()) file.Save(stream);
        }

        public static void Save(string path, Project project)
        {
            new SaveProject(project).Save(path);
        }
    }
}
