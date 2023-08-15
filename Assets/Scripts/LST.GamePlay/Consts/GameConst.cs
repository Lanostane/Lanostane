using Unity.Burst;
using UnityEngine;

namespace LST.GamePlay
{
    public struct GameConst
    {
        public const float SpaceStart = 1.17f;
        public const float SpaceEnd = 10.0f;

        public const float MinNoteSize = 0.112f;
        public const float MaxNoteSize = 1.0f;

        public static float LerpNoteSizeFactor(float t)
        {
            return Mathf.Lerp(MinNoteSize, MaxNoteSize, t);
        }

        public static Vector3 LerpNoteSize(float t)
        {
            return Mathf.Lerp(MinNoteSize, MaxNoteSize, t) * Vector3.one;
        }

        public static float LerpSpaceFactor(float t)
        {
            return Mathf.Lerp(SpaceStart, SpaceEnd, t);
        }
    }
}
