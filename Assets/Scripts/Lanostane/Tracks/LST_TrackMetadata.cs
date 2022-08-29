using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lst.Tracks
{
    public sealed class LST_TrackMetadata
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string MainCharter { get; set; }
        public string MainBG { get; set; }
        public LST_TrackBGAInfo[] BG { get; set; } = Array.Empty<LST_TrackBGAInfo>();
        public LST_TrackChartInfo[] Charts { get; set; } = Array.Empty<LST_TrackChartInfo>();

        public void LoadTrack(string basePath)
        {
            
        }
    }
}
