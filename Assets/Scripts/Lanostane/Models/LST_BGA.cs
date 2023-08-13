using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZeroFormatter;
using PreserveAttribute = UnityEngine.Scripting.PreserveAttribute;
using ID = ZeroFormatter.IndexAttribute;

namespace Lanostane.Models
{
    [Preserve]
    [ZeroFormattable]
    public class LST_BGA
    {
        [ID(0)] public virtual List<LST_BGAItem> BGAFilePath { get; protected set; } = new();
        [ID(100)] public virtual List<LST_BGA_ScaleMotion> ScaleMos { get; protected set; } = new();
        [ID(100)] public virtual List<LST_BGA_ShakeMotion> ShakeMos { get; protected set; } = new();
        [ID(101)] public virtual List<LST_BGA_AlphaMotion> AlphaMos { get; protected set; } = new();
    }

    [Preserve]
    [ZeroFormattable]
    public class LST_BGAItem
    {
        [ID(0)] public virtual string FilePath { get; set; } = string.Empty;
        [ID(1)] public virtual BGAFileType FileType { get; set; } = BGAFileType.Image;
        [ID(2)] public virtual float DefaultXScale { get; set; } = 1.0f;
        [ID(3)] public virtual float DefaultYScale { get; set; } = 1.0f;
    }

    [Preserve]
    [ZeroFormattable]
    public class LST_BGA_ScaleMotion
    {
        [ID(0)] public virtual ushort BGAIndex { get; set; }
        [ID(1)] public virtual float Timing { get; set; }
        [ID(2)] public virtual float Duration { get; set; }
        [ID(3)] public virtual float NewXScale { get; set; }
        [ID(4)] public virtual float NewYScale { get; set; }
        [ID(5)] public virtual LST_Ease Ease { get; set; }
    }

    [Preserve]
    [ZeroFormattable]
    public class LST_BGA_ShakeMotion
    {
        [ID(0)] public virtual ushort BGAIndex { get; set; }
        [ID(1)] public virtual float Timing { get; set; }
        [ID(2)] public virtual float Duration { get; set; }
        [ID(3)] public virtual float NewXScale { get; set; }
        [ID(4)] public virtual float NewYScale { get; set; }
        [ID(5)] public virtual float NewSpeed { get; set; }
        [ID(6)] public virtual LST_Ease Ease { get; set; }
    }

    [Preserve]
    [ZeroFormattable]
    public class LST_BGA_AlphaMotion
    {
        [ID(0)] public virtual ushort BGAIndex { get; set; }
        [ID(1)] public virtual float Timing { get; set; }
        [ID(2)] public virtual float Duration { get; set; }
        [ID(3)] public virtual float NewAlpha { get; set; }
        [ID(4)] public virtual LST_Ease Ease { get; set; }
    }
}
