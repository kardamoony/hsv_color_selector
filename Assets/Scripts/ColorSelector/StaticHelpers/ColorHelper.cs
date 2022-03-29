using UnityEngine;

namespace ColorSelector.StaticHelpers
{
    public static class ColorHelper
    {
        public static float Angle360ToHue(float angle)
        {
            angle = Mathf.Clamp(angle, 0f, 360f);
            return angle.Remap01(0f, 360f);
        }
    }
}
