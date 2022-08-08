namespace Charts
{
    public class LST_DefaultMotion
    {
        public float Degree = 0.0f;
        public float Radius = 0.0f;
        public float Rotation = 0.0f;
        public float Height = -20.0f;
    }

    public class LST_RotationMotion
    {
        public float Timing;
        public float Duration;
        public float DeltaRotation;
        public LST_Ease Ease;
    }

    public class LST_XYLinearMotion
    {
        public float Timing;
        public float Duration;
        public float NewDegree;
        public float NewRadius;
        public LST_Ease Ease;
    }

    public class LST_XYCirclerMotion
    {
        public float Timing;
        public float Duration;
        public float DeltaDegree;
        public float DeltaRadius;
        public LST_Ease Ease;
    }

    public class LST_HeightMotion
    {
        public float Timing;
        public float Duration;
        public float DeltaHeight;
        public LST_Ease Ease;
    }

    public class LST_BPMChange
    {
        public float Timing;
        public float BPM;
    }

    public class LST_ScrollChange
    {
        public float Timing;
        public float Speed;
    }
}
