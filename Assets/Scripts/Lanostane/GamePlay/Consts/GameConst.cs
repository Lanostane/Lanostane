using UnityEngine;

namespace Lst.GamePlay
{
    public struct GameConst
    {
        public const float SpaceStart = 1.17f;
        public const float SpaceEnd = 10.0f;

        public const float MinNoteSize = 0.112f;
        public const float MaxNoteSize = 1.0f;

        public static float LerpNoteSize(float t)
        {
            return Mathf.Lerp(MinNoteSize, MaxNoteSize, t);
        }

        public static float LerpSpace(float t)
        {
            return Mathf.Lerp(SpaceStart, SpaceEnd, t);
        }
    }
}
