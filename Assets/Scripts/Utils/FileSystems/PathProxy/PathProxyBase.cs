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
            var path = Path.Combine(BasePath, fileName);
            File.WriteAllText(path, content);
        }

        public string ReadTextFile(string fileName)
        {
            var path = Path.Combine(BasePath, fileName);
            return File.ReadAllText(path);
        }

        public byte[] ReadBinFile(string fileName)
        {
            var path = Path.Combine(BasePath, fileName);
            return File.ReadAllBytes(path);
        }

        public FileStream Open(string fileName, FileMode mode)
        {
            var path = Path.Combine(BasePath, fileName);
            return File.Open(path, mode);
        }
    }
}
