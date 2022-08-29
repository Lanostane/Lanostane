using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils.FileSystems.PathProxy
{
    public sealed class ChartsPathProxy : PathProxyBase
    {
        public override bool AutoCreatePath => true;
        private readonly ConcurrentDictionary<string, ChartFolderProxy> _SubFolders = new();

        public ChartFolderProxy this[string pathName]
        {
            get
            {
                var lowerPath = pathName.ToLowerInvariant();
                if (_SubFolders.TryGetValue(lowerPath, out var result))
                {
                    return result;
                }
                else
                {
                    var pathProxy = new ChartFolderProxy(pathName);
                    result = _SubFolders[lowerPath] = pathProxy;
                    return result;
                }
            }
        }

        protected override string BuildBasePath()
        {
            return Path.Combine(Paths.PersistentDataPath, "Charts");
        }
    }

    public sealed class ChartFolderProxy : PathProxyBase
    {
        public override bool AutoCreatePath => true;
        public string SubPathName { get; private set; }

        public ChartFolderProxy(string pathName)
        {
            if (string.IsNullOrWhiteSpace(pathName))
            {
                throw new ArgumentException("SubPath cannot be empty or null string!", nameof(pathName));
            }

            SubPathName = pathName;
            Setup();
        }

        protected override string BuildBasePath()
        {
            return Path.Combine(Paths.PersistentDataPath, "Charts", SubPathName);
        }
    }
}
