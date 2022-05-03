using UnityEngine;

namespace StaticHelpers
{
    public static class ColorHelper
    {
        public static float Angle360ToHue(float angle)
        {
            angle = Mathf.Clamp(angle, 0f, 360f);
            return angle.Remap01(0f, 360f);
        }

        public static bool Approximately(this Color thisColor, Color color)
        {
            var rApproximately = Mathf.Approximately(thisColor.r, color.r);
            var gApproximately = Mathf.Approximately(thisColor.g, color.g);
            var bApproximately = Mathf.Approximately(thisColor.b, color.b);
            var aApproximately = Mathf.Approximately(thisColor.a, color.a);
            
            return rApproximately && gApproximately && bApproximately && aApproximately;
        }
    }
}
