using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Lanostane.Tracks
{
    public enum LST_ChartAppearancePreset
    {
        None,
        Whisper,
        Acoustic,
        Ultra,
        Master,
        Orchestral
    }

    public struct LST_ChartAppearance
    {
        public Color Color1 { get; set; }
        public Color Color2 { get; set; }
        public string Prefix { get; set; }

        public static readonly LST_ChartAppearance WhisperPreset = new()
        {
            Color1 = Color.white,
            Color2 = Color.red,
            Prefix = "Whisper"
        };

        public static readonly LST_ChartAppearance AcousticPreset = new()
        {
            Color1 = Color.white,
            Color2 = Color.red,
            Prefix = "Acoustic"
        };

        public static readonly LST_ChartAppearance UltraPreset = new()
        {
            Color1 = Color.white,
            Color2 = Color.red,
            Prefix = "Ultra"
        };

        public static readonly LST_ChartAppearance MasterPreset = new()
        {
            Color1 = Color.white,
            Color2 = Color.red,
            Prefix = "Master"
        };

        public static readonly LST_ChartAppearance OrchestralPreset = new()
        {
            Color1 = Color.white,
            Color2 = Color.red,
            Prefix = "Orchestral"
        };
    }

    public sealed class LST_TrackChartInfo
    {
        public string Charter { get; set; }
        public string ChartFile { get; set; }
        public string StoryboardFile { get; set; }
        public float Difficulty { get; set; }
        public LST_ChartAppearancePreset Preset { get; set; } = LST_ChartAppearancePreset.Master;

        [JsonProperty("Custom")]
        private LST_ChartAppearance Custom { get; set; } = LST_ChartAppearance.MasterPreset;

        public LST_ChartAppearance GetAppearance()
        {
            return Preset switch
            {
                LST_ChartAppearancePreset.Whisper => LST_ChartAppearance.WhisperPreset,
                LST_ChartAppearancePreset.Acoustic => LST_ChartAppearance.AcousticPreset,
                LST_ChartAppearancePreset.Ultra => LST_ChartAppearance.UltraPreset,
                LST_ChartAppearancePreset.Master => LST_ChartAppearance.MasterPreset,
                LST_ChartAppearancePreset.Orchestral => LST_ChartAppearance.OrchestralPreset,
                _ => Custom,
            };
        }
    }
}
