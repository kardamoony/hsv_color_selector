using UnityEngine;

namespace ColorPicker.StaticHelpers
{
    public static class ColorHelper
    {
        public static Color Angle360ToColor(float angle)
        {
            angle = Mathf.Clamp(angle, 0f, 360f);
            var hue = angle.Remap01(0f, 360f);
            return Color.HSVToRGB(hue, 1f, 1f);
        }

        public static float Angle360ToHue(float angle)
        {
            angle = Mathf.Clamp(angle, 0f, 360f);
            return angle.Remap01(0f, 360f);
        }
    }
}
