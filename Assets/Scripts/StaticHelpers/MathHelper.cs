using UnityEngine;

namespace StaticHelpers
{
    public static class MathHelper
    {
        public static float GetAngle360(Vector2 position, Vector2 center, Vector2 originalRotation)
        {
            var dir = (position - center).normalized;
            var signedAngle = -Vector2.SignedAngle(originalRotation, dir);
            if (signedAngle < 0) signedAngle = 360 + signedAngle;
            return signedAngle;
        }

        public static Vector3 RotateCoords(Vector3 coords, Vector3 center, Quaternion rotation)
        {
            var matrix = Matrix4x4.Rotate(rotation);
            var centerOffset = coords - center;
            var rotated = matrix * centerOffset;
            return new Vector3(rotated.x, rotated.y, rotated.z) + center;
        }
        
        public static float Remap01(this float value, float fromMin, float fromMax)
        {
            return (value - fromMin) / (fromMax - fromMin);
        }

        public static float Remap(this float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            return (toMax - toMin) * ((value - fromMin) / (fromMax - fromMin)) + toMin;
        }
    }
}
