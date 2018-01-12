namespace XFramework.Utility
{
    public static class Utility
    {
        public const float Epsilon = 1.0e-7f;
        public static bool IsNotZero(float value)
        {
            return (value < -Epsilon) || (value > Epsilon);
        }
        public static bool IsZero(float value)
        {
            return (value >= -Epsilon) && (value <= Epsilon);
        }
        public static bool AbsIsOverThreshold(float value, float threshold)
        {
            return (value < -threshold) || (value > threshold);
        }
    }
}