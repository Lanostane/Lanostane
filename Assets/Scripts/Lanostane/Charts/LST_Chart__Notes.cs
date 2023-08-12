using System;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;
using UnityEditor;
using UnityEngine.Scripting;

namespace Lanostane.Charts
{
    [Preserve]
    public class LST_Tap : ILST_SingleNote
    {
        public LST_NoteSize Size;
        public float Timing;
        public float Degree;
        public ushort ScrollGroup;
        public bool Highlight;
        public LST_NoteSpecialFlags Flags;

        public LST_SingleNoteInfo NoteInfo => new()
        {
            Timing = Timing,
            Degree = Degree,
            ScrollGroup = ScrollGroup,
            Size = Size,
            Highlight = Highlight,
            Type = LST_SingleNoteType.Click,
            Flags = Flags
        };
    }

    [Preserve]
    public class LST_Catch : ILST_SingleNote
    {
        public LST_NoteSize Size;
        public float Timing;
        public float Degree;
        public ushort ScrollGroup;
        public bool Highlight;
        public LST_NoteSpecialFlags Flags;

        public LST_SingleNoteInfo NoteInfo => new()
        {
            Timing = Timing,
            Degree = Degree,
            ScrollGroup = ScrollGroup,
            Size = Size,
            Highlight = Highlight,
            Type = LST_SingleNoteType.Catch,
            Flags = Flags
        };
    }

    [Preserve]
    public class LST_Flick : ILST_SingleNote
    {
        public LST_NoteSize Size;
        public float Timing;
        public float Degree;
        public ushort ScrollGroup;
        public bool Highlight;
        public LST_FlickDir Direction;
        public LST_NoteSpecialFlags Flags;

        public LST_SingleNoteInfo NoteInfo => new()
        {
            Timing = Timing,
            Degree = Degree,
            ScrollGroup = ScrollGroup,
            Size = Size,
            Highlight = Highlight,
            Type = Direction == LST_FlickDir.In ? LST_SingleNoteType.FlickIn : LST_SingleNoteType.FlickOut,
            Flags = Flags
        };
    }

    [Preserve]
    public class LST_Hold : ILST_LongNote
    {
        public LST_NoteSize Size;
        public float Timing;
        public float Duration;
        public float Degree;
        public ushort ScrollGroup;
        public bool Highlight;
        public LST_NoteSpecialFlags Flags;

        public LST_Joint[] Joints;

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
                Flags = Flags
            };
            info.SetJoints(Joints);
            return info;
        }
    }

    [Preserve]
    public class LST_TraceLine : ILST_LongNote
    {
        public float Timing;
        public float Duration;
        public float Degree;
        public ushort ScrollGroup;
        public LST_Joint[] Joints;

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
                Flags = LST_NoteSpecialFlags.None
            };
            info.SetJoints(Joints);
            return info;
        }
    }

    [Preserve]
    public class LST_Joint
    {
        public float Duration;
        public float DeltaDegree;
        public LST_Ease Ease;
    }

    public enum LST_NoteSize : byte
    {
        Size0,
        Size1,
        Size2
    }

    public static class NoteSizeExtension
    {
        public static LST_NoteSize ToValidSize(this LST_NoteSize size)
        {
            if ((int)size > 2)
                return LST_NoteSize.Size2;

            if ((int)size < 0)
                return LST_NoteSize.Size0;

            return size;
        }
    }

    public enum LST_SingleNoteType : byte
    {
        Click,
        Catch,
        FlickIn,
        FlickOut
    }

    public enum LST_LongNoteType : byte
    {
        Hold,
        Trace
    }

    public enum LST_FlickDir : byte
    {
        In,
        Out
    }

    [Flags]
    public enum LST_NoteSpecialFlags : byte
    {
        None = 0,
        NoJudgement = 1,
        NoGraphic = 2
    }
}
