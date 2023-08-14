namespace Lanostane.Models
{
    public struct LST_SingleNoteInfo
    {
        public float Timing;
        public float Degree;
        public ushort ScrollGroup;
        public LST_SingleNoteType Type;
        public LST_NoteSize Size;
        public bool Highlight;

        public LST_NoteSpecialFlags Flags;
        public LST_ColorPaletteIndex ColorPaletteIndex; //TODO: Implement ColorPalette System;
    }
}
