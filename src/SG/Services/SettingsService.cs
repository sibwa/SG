using System;
using System.IO;
using System.Xml.Linq;

namespace SG.Services
{
    public sealed class SettingsService
    {
        private readonly string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SG", "settings.xml");
        public string SavedId { get; private set; } = "";
        public bool OnlineEnabled { get; private set; } = true;
        public void Load() {
            try {
                if (!File.Exists(path)) return;
                var x=XDocument.Load(path);
                SavedId=(string)x.Root.Element("SavedId") ?? "";
                OnlineEnabled=(bool?)x.Root.Element("OnlineEnabled") ?? true;
            } catch { }
        }
        public void Save(string id, bool online) {
            SavedId=id ?? ""; OnlineEnabled=online;
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            new XDocument(new XElement("Settings",new XElement("SavedId",SavedId),new XElement("OnlineEnabled",OnlineEnabled))).Save(path);
        }
    }
}