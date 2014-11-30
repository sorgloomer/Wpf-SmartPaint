using SmartPaint.Model;
using SmartPaint.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPaint.Persistence
{
    public class FilePersistence
    {
        public Project Project { get; set; }

        public bool OpenFromFile()
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                SetProjectDialog(dlg);
                Nullable<bool> result = dlg.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    this.Project = SptFile.From(dlg.FileName);
                    return true;
                }
            }
            catch (Exception)
            {
                StaticLogger.Error("There was an error while opening the file!");
            }
            return false;
        }

        public bool SaveToFile()
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            SetProjectDialog(dlg);
            Nullable<bool> result = dlg.ShowDialog();
            if (result.HasValue && result.Value)
            {
                SptFile.Save(dlg.FileName, this.Project);
                return true;
            }
            return false;
        }

        private static void SetProjectDialog(Microsoft.Win32.FileDialog dlg)
        {
            dlg.FileName = "MyProject";
            dlg.DefaultExt = ".spt";
            dlg.Filter = "Smart Paint project files (.spt)|*.spt";
        }

    }
}
