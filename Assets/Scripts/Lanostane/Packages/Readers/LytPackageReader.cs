using Cysharp.Threading.Tasks;
using Lst.Tracks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utils.FileSystems;
using Utils.FileSystems.PathProxy;
using Utils.Jsons;

namespace Lst.Packages.Readers
{
    public sealed class LytPackageReader
    {
        public static async UniTask ConvertToLSTAsync(string file)
        {
            using var zip = ZipFile.OpenRead(file);

            await Task.Run(() =>
            {
                try
                {
                    ConvertToLST(file);
                }
                catch
                {

                }
            });
        }

        public static void ConvertToLST(string file)
        {
            try
            {
                using ZipArchive zip = ZipFile.Open(file, ZipArchiveMode.Read);
                if (!TryReadInfoBytes(zip, out var meta))
                {
                    throw new FileNotFoundException("info.bytes were not found on layesta file");
                }

                if (!TryWriteFolder(zip, meta))
                {
                    throw new FileNotFoundException("Unable to write a file to folder");
                }
            }
            catch(Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }

        private static bool TryReadInfoBytes(ZipArchive zip, out LST_TrackMetadata meta)
        {
            try
            {
                var infoEntry = zip.GetEntry("info.bytes");
                using var infoReader = new BinaryReader(infoEntry.Open());
                meta = new LST_TrackMetadata();
                //string - Name
                //string - Designer
                //string - ChartCount
                //string[ChartCount] - ChartName
                //int - Image Width
                //int - Image Height
                //string - Artist (Why the fuck is this here)
                //int - Version? (Always 2)
                meta.Title = infoReader.ReadString();

                var charter = infoReader.ReadString();
                meta.MainCharter = charter;

                var chartCount = infoReader.ReadInt32();
                var chartList = new List<LST_TrackChartInfo>();
                for (int i = 0; i < chartCount; i++)
                {
                    var chartName = infoReader.ReadString();
                    chartList.Add(new()
                    {
                        Charter = charter,
                        ChartFile = chartName,
                        Preset = LST_ChartAppearancePreset.Master,
                        StoryboardFile = string.Empty,
                        Difficulty = float.NaN
                    });
                }
                meta.Charts = chartList.ToArray();
                var imageDimensionX = infoReader.ReadInt32();
                var imageDimensionY = infoReader.ReadInt32();
                meta.Artist = infoReader.ReadString();
                var version = infoReader.ReadInt32();
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
                meta = null;
                return false;
            }
        }

        private static bool TryWriteFolder(ZipArchive zip, LST_TrackMetadata meta)
        {
            try
            {
                var basePath = Paths.Charts[$"[Lyt] {meta.Artist} - {meta.Title} by {meta.MainCharter}"];
                basePath.DeleteAll();

                foreach (var chart in meta.Charts)
                {
                    var origName = chart.ChartFile;
                    var origNameBytes = Encoding.UTF8.GetBytes(origName);
                    var chartFileName = $"chart_{Convert.ToBase64String(origNameBytes)}.txt";
                    chart.ChartFile = $"chart_{origName}.txt";
                    var chartEntry = zip.GetEntry(chartFileName);
                    if (chartEntry == null)
                    {
                        Debug.LogError($"File is missing in Zip! {chartFileName}");
                        foreach (var entry in zip.Entries)
                        {
                            Debug.LogError(entry.FullName);
                        }
                        continue;
                    }
                    chartEntry.ExtractToFile(Path.Combine(basePath.BasePath, chart.ChartFile), true);
                }

                TryUnzipToPath(zip, basePath, "music.mp3", "music.mp3");

                var bgaList = new List<LST_TrackBGAInfo>();
                var flag = TryUnzipToPath(zip, basePath, "background_linear.jpg", "bga_0.jpg");
                if (flag)
                {
                    bgaList.Add(new()
                    {
                        File = "bga_0.jpg",
                        Weight = 1.0f
                    });
                }
                flag = TryUnzipToPath(zip, basePath, "background_gray.jpg", "bga_1.jpg");
                if (flag)
                {
                    bgaList.Add(new()
                    {
                        File = "bga_1.jpg",
                        Weight = 1.0f
                    });
                }
                flag = TryUnzipToPath(zip, basePath, "background.jpg", "bga.jpg");
                if (flag)
                {
                    bgaList.Add(new()
                    {
                        File = "bga.jpg",
                        Weight = 1.0f
                    });
                }

                meta.BG = bgaList.ToArray();

                var json = UnityJSON.Serialize(meta);
                basePath.WriteTextFile(LST_Package.MetaFileName, json);

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
                return false;
            }
        }

        private static bool TryUnzipToPath(ZipArchive zip, ChartFolderProxy path, string entryName, string newName)
        {
            var entry = zip.GetEntry(entryName);
            if (entry == null)
                return false;

            entry.ExtractToFile(Path.Combine(path.BasePath, newName), true);
            return true;
        }
    }
}
