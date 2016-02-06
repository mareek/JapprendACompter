using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace JapprendACompter.Stats
{
    public class StatFile
    {
        private const string StatFileName = "stats.xml";

        private static Lazy<Task<StatFile>> LazyFile = new Lazy<Task<StatFile>>(async () => new StatFile(await GetDocumentAsync()));
        public static StatFile Instance => LazyFile.Value.Result;

        private StatFile(XDocument document)
        {
            Sessions = document.Root.Descendants("Session").Select(e => new Session(e)).ToList();
        }

        public List<Session> Sessions { get; }

        public Session NewSession(string exercice)
        {
            var session = new Session(exercice);
            Sessions.Insert(0, session);
            return session;
        }

        public async Task Save()
        {
            var statFile = await TryGetStatFileAsync() ?? await ApplicationData.Current.LocalFolder.CreateFileAsync(StatFileName);

            using (var statFileStream = await statFile.OpenStreamForWriteAsync())
            {
                ToXml().Save(statFileStream);
            }
        }

        public XDocument ToXml() => new XDocument(
                                        new XElement("Session",
                                            Sessions.Select(s => s.ToXml())));

        public static Task LoadFileAsync() => LazyFile.Value;

        private static async Task<XDocument> GetDocumentAsync()
        {
            var statFile = await TryGetStatFileAsync();
            if (statFile == null)
            {
                return new XDocument(new XElement("Sessions"));
            }

            using (var statFileStream = await statFile.OpenStreamForReadAsync())
            {
                return XDocument.Load(statFileStream);
            }
        }

        private static async Task<StorageFile> TryGetStatFileAsync() => await ApplicationData.Current.LocalFolder.TryGetItemAsync(StatFileName) as StorageFile;
    }
}
