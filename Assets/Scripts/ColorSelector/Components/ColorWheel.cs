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

        protected override Vector3 ValueToCursorPosition(Vector3 center, float value)
        {
            var (cursorDistance, _, _) = GetCursorDistance(center);
            var angle = 360f - Mathf.Clamp01(value).Remap(0f, 1f, 0f, 360f);
            var zeroPosition = center + Vector3.up * cursorDistance;
            return MathHelper.RotateCoords(zeroPosition, center, Quaternion.Euler(Vector3.forward * angle));
            //var cursorDirection = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;
            //return center + cursorDirection * cursorDistance;
        }

        protected override bool IsClickValid(Vector3? position, Vector3 center, out Vector3 cursorPosition, out float colorValue)
        {
            cursorPosition = Vector3.zero;
            colorValue = 1f;
            
            if (!position.HasValue) return false;

            var pos = position.Value;

            var (cursorDistance, innerRadius, outerRadius) = GetCursorDistance(center);
            
            var cursorDirection = (pos - center).normalized;
            cursorPosition = center + cursorDirection * cursorDistance;
            
            var clickDistance = Vector3.Distance(center, pos);

            var isValid = clickDistance >= innerRadius && clickDistance <= outerRadius;
            
            var angle = MathHelper.GetAngle360(pos, center, Vector2.up);
            colorValue = ColorHelper.Angle360ToHue(angle);

            return isValid;
        }

        protected override void OnMaterialSetup(Material material)
        {
            base.OnMaterialSetup(material);
            material.SetFloat(BackgroundCircle, _backgroundSize);
        }
    }
}
