using Lanostane.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utils.FileSystems.PathProxy;

namespace Lanostane.Packages
{
    public class LST_Package
    {
        public const string MetaFileName = "meta.json";
        public const string MusicFileName = "music.mp3";
        public const string PreviewFileName = "preview.mp3";

        public LST_TrackMetadata Metadata;
        public ChartFolderProxy PathHandle;

        public void LoadSelectionAssets()
        {
            
        }

        private bool TryLoadMP3Audio(string fileName, out AudioClip clip)
        {
            clip = null;
            return true;
        }

        private bool TryLoadImage(string fileName, out Texture2D texture)
        {
            try
            {
                var bytes = PathHandle.ReadBinFile(fileName);
                texture = new Texture2D(1, 1);
                if (!texture.LoadImage(bytes, false))
                {
                    UnityEngine.Object.Destroy(texture);
                    return false;
                }
                return true;
            }
            catch
            {
                texture = null;
                return false;
            }
        }
    }
}
