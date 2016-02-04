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

        private static Lazy<Task<StatFile>> LazyFile = new Lazy<Task<StatFile>>(async () => new StatFile(await LoadFile()));

        public Task<StatFile> Instance => LazyFile.Value;

        private static async Task<XDocument> LoadFile()
        {
            var statFile = await GetStatFileAsync();
            if (statFile == null)
            {
                return new XDocument(new XElement("Sessions"));
            }

            using (var statFileStream = await statFile.OpenStreamForReadAsync())
            {
                return XDocument.Load(statFileStream);
            }
        }

        private readonly XDocument _document;

        private StatFile(XDocument document)
        {
            _document = document;
            Sessions = document.Root.Descendants("Session").Select(e => new Session(e)).ToList();
        }

        public List<Session> Sessions { get; }

        public async Task Save()
        {
            var statFile = await GetStatFileAsync() ?? await ApplicationData.Current.LocalFolder.CreateFileAsync(StatFileName);

            using (var statFileStream = await statFile.OpenStreamForWriteAsync())
            {
                _document.Save(statFileStream);
            }
        }

        private static async Task<StorageFile> GetStatFileAsync() => await ApplicationData.Current.LocalFolder.TryGetItemAsync(StatFileName) as StorageFile;
    }
}
