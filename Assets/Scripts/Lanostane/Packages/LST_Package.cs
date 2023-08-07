using Cysharp.Threading.Tasks;
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

        public Texture2D MainBG;

        public async UniTask LoadSelectionAssets(Action onDone)
        {
            MainBG = await LoadImage(Metadata.MainBG);
        }

        public void UnloadSelectionAssets()
        {

        }

        private async UniTask<Texture2D> LoadImage(string fileName)
        {
            try
            {
                var bytes = await PathHandle.ReadBinFileAsync(fileName);
                var texture = new Texture2D(1, 1);
                if (!texture.LoadImage(bytes, false))
                {
                    UnityEngine.Object.Destroy(texture);
                    return null;
                }
                return texture;
            }
            catch
            {
                return null;
            }
        }

        //TODO: Mp3 Reading
        private async UniTask<AudioClip> LoadMp3Audio(string fileName)
        {
            try
            {
                var bytes = await PathHandle.ReadBinFileAsync(fileName);
                var audio = AudioClip.Create("", 1, 1, 1, false);
                return audio;
            }
            catch
            {
                return null;
            }
        }
    }
}
