using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lanostane.Models
{
    public enum LST_NoteSize : byte
    {
        Size0,
        Size1,
        Size2
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
}
