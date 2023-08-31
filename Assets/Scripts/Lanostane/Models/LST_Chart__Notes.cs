using System;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;
using UnityEngine.Scripting;
using ZeroFormatter;
using System.Collections.Generic;
using System.Linq;

using PreserveAttribute = UnityEngine.Scripting.PreserveAttribute;
using ID = ZeroFormatter.IndexAttribute;
using Utils.Maths;

namespace Lanostane.Models
{
    [Preserve]
    [ZeroFormattable]
    public class LST_Tap : ILST_SingleNote
    {
        [ID(0)] public virtual LST_NoteSize Size { get; set; }
        [ID(1)] public virtual float Timing { get; set; }
        [ID(2)] public virtual float Degree { get; set; }
        [ID(3)] public virtual ushort ScrollGroup { get; set; }
        [ID(4)] public virtual bool Highlight { get; set; }

        [ID(100)] public virtual LST_NoteSpecialFlags Flags { get; set; } = LST_NoteSpecialFlags.None;
        [ID(101)] public virtual LST_ColorPaletteIndex ColorPaletteIndex { get; set; } = LST_ColorPaletteIndex.None;

        [IgnoreFormat]
        public LST_SingleNoteInfo NoteInfo => new()
        {
            Timing = Timing,
            Degree = Degree,
            ScrollGroup = ScrollGroup,
            Size = Size,
            Highlight = Highlight,
            Type = LST_SingleNoteType.Click,

            Flags = Flags,
            ColorPaletteIndex = ColorPaletteIndex
        };
    }

    [Preserve]
    [ZeroFormattable]
    public class LST_Catch : ILST_SingleNote
    {
        [ID(0)] public virtual LST_NoteSize Size { get; set; }
        [ID(1)] public virtual float Timing { get; set; }
        [ID(2)] public virtual float Degree { get; set; }
        [ID(3)] public virtual ushort ScrollGroup { get; set; }
        [ID(4)] public virtual bool Highlight { get; set; }

        [ID(100)] public virtual LST_NoteSpecialFlags Flags { get; set; } = LST_NoteSpecialFlags.None;
        [ID(101)] public virtual LST_ColorPaletteIndex ColorPaletteIndex { get; set; } = LST_ColorPaletteIndex.None;

        [IgnoreFormat]
        public LST_SingleNoteInfo NoteInfo => new()
        {
            Timing = Timing,
            Degree = Degree,
            ScrollGroup = ScrollGroup,
            Size = Size,
            Highlight = Highlight,
            Type = LST_SingleNoteType.Catch,

            Flags = Flags,
            ColorPaletteIndex = ColorPaletteIndex
        };
    }

    [Preserve]
    [ZeroFormattable]
    public class LST_Flick : ILST_SingleNote
    {
        [ID(0)] public virtual LST_NoteSize Size { get; set; }
        [ID(1)] public virtual float Timing { get; set; }
        [ID(2)] public virtual float Degree { get; set; }
        [ID(3)] public virtual ushort ScrollGroup { get; set; }
        [ID(4)] public virtual bool Highlight { get; set; }
        [ID(5)] public virtual LST_FlickDir Direction { get; set; }

        [ID(100)] public virtual LST_NoteSpecialFlags Flags { get; set; } = LST_NoteSpecialFlags.None;
        [ID(101)] public virtual LST_ColorPaletteIndex ColorPaletteIndex { get; set; } = LST_ColorPaletteIndex.None;

        [IgnoreFormat]
        public LST_SingleNoteInfo NoteInfo => new()
        {
            Timing = Timing,
            Degree = Degree,
            ScrollGroup = ScrollGroup,
            Size = Size,
            Highlight = Highlight,
            Type = Direction == LST_FlickDir.In ? LST_SingleNoteType.FlickIn : LST_SingleNoteType.FlickOut,

            Flags = Flags,
            ColorPaletteIndex = ColorPaletteIndex
        };
    }

    [Preserve]
    [ZeroFormattable]
    public class LST_Hold : ILST_LongNote
    {
        [ID(0)] public virtual LST_NoteSize Size { get; set; }
        [ID(1)] public virtual float Timing { get; set; }
        [ID(2)] public virtual float Duration { get; set; }
        [ID(3)] public virtual float Degree { get; set; }
        [ID(4)] public virtual ushort ScrollGroup { get; set; }
        [ID(5)] public virtual bool Highlight { get; set; }
        [ID(6)] public virtual List<LST_Joint> Joints { get; protected set; } = new();

        [ID(100)] public virtual LST_NoteSpecialFlags Flags { get; set; } = LST_NoteSpecialFlags.None;
        [ID(101)] public virtual LST_ColorPaletteIndex ColorPaletteIndex { get; set; } = LST_ColorPaletteIndex.None;

        [IgnoreFormat]
        public LST_LongNoteInfo NoteInfo => CreateNoteInfo();
        public LST_LongNoteInfo CreateNoteInfo()
        {
            var info = new LST_LongNoteInfo()
            {
                Timing = Timing,
                Duration = Duration,
                Degree = Degree,
                ScrollGroup = ScrollGroup,
                Size = Size,
                Highlight = Highlight,
                Type = LST_LongNoteType.Hold,

                Flags = Flags,
                ColorPaletteIndex = ColorPaletteIndex
            };
            info.SetJoints(Joints.ToArray());
            return info;
        }
    }

    [Preserve]
    [ZeroFormattable]
    public class LST_TraceLine : ILST_LongNote
    {
        [ID(0)] public virtual float Timing { get; set; }
        [ID(1)] public virtual float Duration { get; set; }
        [ID(2)] public virtual float Degree { get; set; }
        [ID(3)] public virtual ushort ScrollGroup { get; set; }
        [ID(4)] public List<LST_Joint> Joints { get; protected set; } = new();

        [ID(100)] public virtual LST_ColorPaletteIndex ColorPaletteIndex { get; set; } = LST_ColorPaletteIndex.None;

        [IgnoreFormat]
        public LST_LongNoteInfo NoteInfo => CreateNoteInfo();
        public LST_LongNoteInfo CreateNoteInfo()
        {
            var info = new LST_LongNoteInfo()
            {
                Timing = Timing,
                Duration = Duration,
                Degree = Degree,
                ScrollGroup = ScrollGroup,
                Size = LST_NoteSize.Size0,
                Highlight = false,
                Type = LST_LongNoteType.Hold,

                Flags = LST_NoteSpecialFlags.None,
                ColorPaletteIndex = ColorPaletteIndex
            };
            info.SetJoints(Joints.ToArray());
            return info;
        }
    }

    [Preserve]
    [ZeroFormattable]
    public class LST_Joint
    {
        [ID(0)] public virtual float Duration { get; set; }
        [ID(1)] public virtual float DeltaDegree { get; set; }
        [ID(2)] public virtual EaseType Ease { get; set; }
    }
}
