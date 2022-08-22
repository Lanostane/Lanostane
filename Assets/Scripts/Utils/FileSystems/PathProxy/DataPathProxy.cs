using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils.FileSystems.PathProxy
{
    public class DataPathProxy : PathProxyBase
    {
        public override bool AutoCreatePath => true;

        protected override string BuildBasePath()
        {
            return Path.Combine(Application.persistentDataPath, "Data");
        }
    }
}
