using ColorSelector.StaticHelpers;
using UnityEngine;

namespace ColorSelector.Components
{
    [ExecuteInEditMode]
    public class ColorWheel : CircularColorValueController
    {
        private static readonly int BackgroundCircle = Shader.PropertyToID("_BackgroundCircle");

        [Range(0, 1)] 
        [SerializeField] private float _backgroundSize = 1f;

        protected override ColorCursorData ValueToCursorData(Vector3 center, float value)
        {
            var (cursorDistance, _, _) = GetCursorDistance(center);
            var angle = Mathf.Clamp01(value).Remap(0f, 1f, 0f, 360f);
            var zeroPosition = center + Vector3.up * cursorDistance;
            var cursorPosition = MathHelper.RotateCoords(zeroPosition, center, Quaternion.Euler(Vector3.forward * (360f - angle)));
            return new ColorCursorData { Position = cursorPosition, Angle = angle, Value = value};
        }
        
        protected override bool IsClickValid(Vector3? position, Vector3 center, out ColorCursorData data)
        {
            if (!position.HasValue)
            {
                data = default;
                return false;
            }

            var pos = position.Value;

            var (cursorDistance, innerRadius, outerRadius) = GetCursorDistance(center);
            
            var cursorDirection = (pos - center).normalized;
            var cursorPosition = center + cursorDirection * cursorDistance;
            
            var clickDistance = Vector3.Distance(center, pos);

            var isValid = clickDistance >= innerRadius && clickDistance <= outerRadius;
            
            var angle = MathHelper.GetAngle360(pos, center, Vector2.up);
            var colorValue = ColorHelper.Angle360ToHue(angle);

            data = new ColorCursorData { Position = cursorPosition, Angle = angle, Value = colorValue };

            return isValid;
        }

        protected override void OnMaterialSetup(Material material)
        {
            base.OnMaterialSetup(material);
            material.SetFloat(BackgroundCircle, _backgroundSize);
        }
    }
}
