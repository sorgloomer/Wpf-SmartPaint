using SmartPaint.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartPaint.Common
{
    public class PluginContainer
    {
        public List<ITransform> Transforms { get; protected set; }

        public PluginContainer()
        {
            this.Transforms = new List<ITransform>();
        }

        public void LoadPluginsDirectory(string path = "plugins")
        {
            foreach (var file in Directory.EnumerateFiles(path, "*.dll"))
            {
                try
                {
                    var fullFilePath = Path.GetFullPath(file);
                    var a = Assembly.LoadFile(fullFilePath);
                    foreach (var type in a.GetTypes().Where(t => typeof(ITransform).IsAssignableFrom(t)))
                    {
                        try
                        {
                            var instance = (ITransform)Activator.CreateInstance(type);
                            this.Transforms.Add(instance);
                        }
                        // Pokemon exception handling: Catch 'em all!
                        catch (Exception)
                        {
                            StaticLogger.Warn(string.Format("Couldn't instantiate plugin {0} in plugin file {1}.", type.FullName, file));
                        }
                    }
                }
                // Pokemon exception handling: Catch 'em all!
                catch (Exception)
                {
                    StaticLogger.Warn(string.Format("Plugin file {0} could not be loaded.", file));
                }
            }
        }

    }
}
