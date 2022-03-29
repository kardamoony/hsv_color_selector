using ColorSelector.StaticHelpers;
using UnityEngine;

namespace ColorSelector.Components
{
    public class CircularColorSlider : CircularColorValueController
    {
        private static readonly int Sector = Shader.PropertyToID("_Sector");
        private static readonly int Rotate = Shader.PropertyToID("_Rotate");

        private static readonly int FlipX = Shader.PropertyToID("_FlipX");
        private static readonly int FlipY = Shader.PropertyToID("_FlipY");
        
        [SerializeField] private float _startAngle = 10;
        [SerializeField] private float _endAngle = 170;
        [SerializeField] private float _clickRadiusTolerance = 0.1f;
        
        [Range(0f, 10f)]
        [SerializeField] private float _clickAngleTolerance = 2f;

        [Space(10f)]
        [SerializeField] private bool _reverseValue;
        [SerializeField] private bool _flipX;
        [SerializeField] private bool _flipY;

        [Space(10f)]
        [SerializeField]private string _colorProperty = "Color0";
        
        public override void OnColorChanged(ColorSelection colorSelection)
        {
            Material.SetColor(_colorProperty, colorSelection.HueColor);
        }

        protected override Vector3 ValueToCursorPosition(Vector3 center, float value)
        {
            var (cursorDistance, _, _) = GetCursorDistance(center);

            value = _reverseValue ? 1f - value : value;
            
            var flipAngle = _flipX ? -1f : 1f;
            var angle = Mathf.Clamp01(value).Remap(0f, 1f, _startAngle, _endAngle);
            var zeroPosition = center + Vector3.up * cursorDistance;
            
            return MathHelper.RotateCoords(zeroPosition, center, Quaternion.Euler(Vector3.forward * angle * flipAngle));
        }

        protected override bool IsClickValid(Vector3? position, Vector3 center, out Vector3 cursorPosition, out float colorValue)
        {
            cursorPosition = Vector3.zero;
            colorValue = 1f;
            
            if (!position.HasValue) return false;

            var pos = position.Value;
            
            var (cursorDistance, innerRadius, outerRadius) = GetCursorDistance(center);
            var zeroPosition = center + Vector3.up * cursorDistance;
            
            var flipAngle = _flipX ? -1f : 1f;
            var angle = MathHelper.GetAngle360(pos, center, Vector2.up);
            
            if (!_flipX)
            {
                angle = 360f - angle;
            }
            
            var angleClamped = Mathf.Clamp(angle, _startAngle, _endAngle);

            cursorPosition = MathHelper.RotateCoords(zeroPosition, center, Quaternion.Euler(Vector3.forward * angleClamped * flipAngle));
            
            var clickDistance = Vector3.Distance(center, pos);

            var isPositionValid = clickDistance >= innerRadius - _clickRadiusTolerance && clickDistance <= outerRadius + _clickRadiusTolerance;
            var isAngleValid = angle >= _startAngle - _clickAngleTolerance && angle <= _endAngle + _clickAngleTolerance;

            var isValid = isAngleValid && isPositionValid;

            colorValue = angleClamped.Remap01(_startAngle, _endAngle);
            if (_reverseValue) colorValue = 1f - colorValue;

            return isValid;
        }

        protected override void OnMaterialSetup(Material material)
        {
            base.OnMaterialSetup(material);
            material.SetFloat(Rotate, _startAngle);
            material.SetFloat(Sector, _endAngle - _startAngle);
            material.SetFloat(FlipX, _flipX ? 1f : 0f);
            material.SetFloat(FlipY, _flipY ? 1f : 0f);
        }
    }
}
