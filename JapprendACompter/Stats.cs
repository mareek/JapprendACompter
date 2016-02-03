using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace JapprendACompter
{
    public class Stats
    {
        private const string StatFileName = "stats.xml";

        private static StorageFolder LocalFolder => ApplicationData.Current.LocalFolder;

        private static async Task<StorageFile> GetStatFileAsync() => await LocalFolder.TryGetItemAsync(StatFileName) as StorageFile;

        private Lazy<Task<XDocument>> _document = new Lazy<Task<XDocument>>(LoadFile);

        private static async Task<XDocument> LoadFile()
        {
            var statFile = await GetStatFileAsync();
            if (statFile == null)
            {
                return new XDocument();
            }

            using (var statFileStream = await statFile.OpenStreamForReadAsync())
            {
                return XDocument.Load(statFileStream);
            }
        }

        public async Task Save()
        {
            var document = await _document.Value;
            var statFile = await GetStatFileAsync() ?? await LocalFolder.CreateFileAsync(StatFileName);

            using (var statFileStream = await statFile.OpenStreamForWriteAsync())
            {
                document.Save(statFileStream);
            }
        }
    }
}
