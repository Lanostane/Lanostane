using System.Collections.Generic;
using UnityEngine.Scripting;
using Utils.Maths;
using ZeroFormatter;
using PreserveAttribute = UnityEngine.Scripting.PreserveAttribute;
using ID = ZeroFormatter.IndexAttribute;

namespace Lanostane.Models
{
    [Preserve]
    [ZeroFormattable]
    public class LST_Chart
    {
        [ID(000)] public virtual float SongLength { get; set; }
        [ID(001)] public virtual float StartBPM { get; set; }
        
        [ID(100)] public virtual List<LST_Tap> TapNotes { get; protected set; } = new();
        [ID(101)] public virtual List<LST_Catch> CatchNotes { get; protected set; } = new();
        [ID(102)] public virtual List<LST_Flick> FlickNotes { get; protected set; } = new();

        [ID(150)] public virtual List<LST_Hold> HoldNotes { get; protected set; } = new();
        [ID(151)] public virtual List<LST_TraceLine> TraceLines { get; protected set; } = new();

        [ID(200)] public virtual LST_DefaultMotion Default { get; protected set; } = new();
        [ID(201)] public virtual List<LST_RotationMotion> RotationMos { get; protected set; } = new();
        [ID(202)] public virtual List<LST_XYLinearMotion> LinearMos { get; protected set; } = new();
        [ID(203)] public virtual List<LST_XYCirclerMotion> CirclerMos { get; protected set; } = new();
        [ID(204)] public virtual List<LST_HeightMotion> HeightMos { get; protected set; } = new();
        [ID(205)] public virtual List<LST_SubCameraMotions> SubCamMos { get; protected set; } = new(); //TODO: Implement SubCamera Motions

        [ID(300)] public virtual List<LST_BPMChange> BPMs { get; protected set; } = new();
        [ID(301)] public virtual List<LST_ScrollChange> Scrolls { get; protected set; } = new();
        [ID(302)] public virtual List<LST_ColorPaletteChange> PaletteSwaps { get; protected set; } = new();

        [ID(400)] public virtual List<LST_Event> Events { get; protected set; } = new();
    }

    [Preserve]
    [ZeroFormattable]
    public class LST_Event
    {
        [ID(0)] public virtual string EventName { get; set; }
        [ID(1)] public virtual string EventParams { get; set; }
        [ID(2)] public virtual float Timing { get; set; }
        [ID(3)] public virtual float Duration { get; set; }
    }
}
