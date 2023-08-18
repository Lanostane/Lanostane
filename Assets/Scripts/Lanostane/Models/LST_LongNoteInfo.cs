using System;
using System.Linq;
using Utils;
using Utils.Maths;

namespace Lanostane.Models
{
    public struct LST_LongNoteInfo
    {
        public float Timing;
        public float Duration;
        public float Degree;
        public ushort ScrollGroup;
        public LST_LongNoteType Type;
        public LST_NoteSize Size;
        public bool Highlight;
        public LST_JointInfo[] Joints;

        public LST_NoteSpecialFlags Flags;
        public LST_ColorPaletteIndex ColorPaletteIndex; //TODO: Implement ColorPalette System;

        public void SetJoints(params LST_Joint[] joints)
        {
            if (joints == null)
            {
                Joints = Array.Empty<LST_JointInfo>();
            }

            var list = TempList<LST_JointInfo>.GetList();
            foreach (var j in joints)
            {
                list.Add(new()
                {
                    Duration = j.Duration,
                    DeltaDegree = j.DeltaDegree,
                    Ease = j.Ease
                });
            }

            Joints = list.ToArray();
        }
    }

    public struct LST_JointInfo
    {
        public float Duration;
        public float DeltaDegree;
        public EaseType Ease;
    }
}
