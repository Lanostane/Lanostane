using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.FileSystems.PathProxy
{
    public abstract class PathProxyBase
    {
        public string BasePath { get; private set; }
        public abstract bool AutoCreatePath { get; }

        private bool _IsSetup = false;

        protected abstract string BuildBasePath();

        public void Setup()
        {
            if (_IsSetup)
                return;

            BasePath = BuildBasePath();

            if (AutoCreatePath && !Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }

            _IsSetup = true;
        }

        public void WriteTextFile(string fileName, string content)
        {
            var path = GetFullFilePath(fileName);
            File.WriteAllText(path, content);
        }

        public string ReadTextFile(string fileName)
        {
            var path = GetFullFilePath(fileName);
            return File.ReadAllText(path);
        }

        public async UniTask<string> ReadTextFileAsync(string fileName)
        {
            var path = GetFullFilePath(fileName);
            return await File.ReadAllTextAsync(path);
        }

        public byte[] ReadBinFile(string fileName)
        {
            var path = GetFullFilePath(fileName);
            return File.ReadAllBytes(path);
        }

        public async UniTask<byte[]> ReadBinFileAsync(string fileName)
        {
            var path = GetFullFilePath(fileName);
            return await File.ReadAllBytesAsync(path);
        }

        public void WriteBinFile(string fileName, byte[] bytes)
        {
            var path = GetFullFilePath(fileName);
            File.WriteAllBytes(path, bytes);
        }

        public void Delete(string fileName)
        {
            var path = GetFullFilePath(fileName);
            File.Delete(path);
        }

        public void DeleteAll()
        {
            foreach(var file in Directory.GetFiles(BasePath))
            {
                File.Delete(file);
            }
        }

        public string GetFullFilePath(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("File name is null or empty or whitespace! This is not allowed!", nameof(fileName));
            }
            return Path.Combine(BasePath, fileName);
        }

        public string GetFullFileURI(string fileName)
        {
            return new Uri(GetFullFilePath(fileName)).AbsoluteUri;
        }

        public FileStream Open(string fileName, FileMode mode)
        {
            var path = Path.Combine(BasePath, fileName);
            return File.Open(path, mode);
        }
    }
}
