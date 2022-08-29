namespace Lanostane.Charts
{
    public class LST_Tap : ILST_SingleNote
    {
        public LST_NoteSize Size;
        public float Timing;
        public float Degree;
        public bool Highlight;

        public LST_SingleNoteInfo NoteInfo => new()
        {
            Timing = Timing,
            Degree = Degree,
            Size = Size,
            Highlight = Highlight,
            Type = LST_SingleNoteType.Click
        };
    }

    public class LST_Catch : ILST_SingleNote
    {
        public LST_NoteSize Size;
        public float Timing;
        public float Degree;
        public bool Highlight;

        public LST_SingleNoteInfo NoteInfo => new()
        {
            Timing = Timing,
            Degree = Degree,
            Size = Size,
            Highlight = Highlight,
            Type = LST_SingleNoteType.Catch
        };
    }

    public class LST_Flick : ILST_SingleNote
    {
        public LST_NoteSize Size;
        public float Timing;
        public float Degree;
        public bool Highlight;
        public LST_FlickDir Direction;

        public LST_SingleNoteInfo NoteInfo => new()
        {
            Timing = Timing,
            Degree = Degree,
            Size = Size,
            Highlight = Highlight,
            Type = Direction == LST_FlickDir.In ? LST_SingleNoteType.FlickIn : LST_SingleNoteType.FlickOut
        };
    }

    public class LST_Hold
    {
        public LST_NoteSize Size;
        public float Timing;
        public float Duration;
        public float Degree;
        public bool Highlight;

        public LST_Joint[] Joints;

        public LST_LongNoteInfo NoteInfo => CreateNoteInfo();
        public LST_LongNoteInfo CreateNoteInfo()
        {
            var info = new LST_LongNoteInfo()
            {
                Timing = Timing,
                Duration = Duration,
                Degree = Degree,
                Size = Size,
                Highlight = Highlight,
                Type = LST_LongNoteType.Hold,
            };
            info.SetJoints(Joints);
            return info;
        }
    }

    public class LST_Joint
    {
        public float Duration;
        public float DeltaDegree;
        public LST_Ease Ease;
    }

    public enum LST_NoteSize
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

    public enum LST_SingleNoteType
    {
        Click,
        Catch,
        FlickIn,
        FlickOut
    }

    public enum LST_LongNoteType
    {
        Hold
    }

    public enum LST_FlickDir
    {
        In,
        Out
    }
}
