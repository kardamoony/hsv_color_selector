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

        [Space(10f)] 
        [SerializeField] private Vector3 _rotationAxis = Vector3.back;
        [SerializeField] private bool _rotateCursor = true;

        protected override ICursorInfo ValueToCursorInfo(Vector3 center, float value)
        {
            var angle = Mathf.Clamp01(value).Remap(0f, 1f, 0f, 360f);
            if (_rotateCursor) return new CursorRotationInfo(angle, _rotationAxis);
            
            var (cursorDistance, _, _) = GetCursorDistance(center);
            var zeroPosition = center + Vector3.up * cursorDistance;
            var cursorPosition = MathHelper.RotateCoords(zeroPosition, center, Quaternion.Euler(_rotationAxis *  angle));
            return new CursorPositionInfo(cursorPosition);
        }

        protected override bool IsClickValid(Vector3? position, Vector3 center, out ICursorInfo positionInfo, out float value)
        {
            if (!position.HasValue)
            {
                positionInfo = default;
                value = 0f;
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

            if (_rotateCursor)
            {
                positionInfo = new CursorRotationInfo(angle, Vector3.back);
            }
            else
            {
                positionInfo = new CursorPositionInfo(cursorPosition);
            }
            
            value = colorValue;

            return isValid;
        }

        protected override void OnMaterialSetup(Material material)
        {
            base.OnMaterialSetup(material);
            material.SetFloat(BackgroundCircle, _backgroundSize);
        }
    }
}
