using StaticHelpers;
using UnityEngine;

namespace HSVColorSelector
{
    public class RingColorSlider : RingColorValueController
    {
        private static readonly int SectorProperty = Shader.PropertyToID("_Sector");
        private static readonly int RotateProperty = Shader.PropertyToID("_Rotate");

        private static readonly int FlipXProperty = Shader.PropertyToID("_FlipX");
        private static readonly int FlipYProperty = Shader.PropertyToID("_FlipY");

        [Range(0f, 360f)]
        [SerializeField] private float _startAngle = 10;
        [Range(0f, 360f)]
        [SerializeField] private float _endAngle = 170;

        [Range(0f, 10f)]
        [SerializeField] private float _clickAngleTolerance = 5f;

        [Space(10f)]
        [SerializeField] private bool _reverseValue;
        [SerializeField] private bool _flipY;
        [SerializeField] private bool _flipX;

        protected override void OnPointerEvent(Vector2 displayPosition, Vector2 validatePosition, float innerCircle, float outerCircle)
        {
            var (innerRadius, outerRadius) = GetRelativeCircleRadii(innerCircle, outerCircle);
            var center = RectTransform.position;
   
            if (!IsClickValid(center, validatePosition, innerRadius, outerRadius)) return;
            
            var angle = MathHelper.GetAngle360(displayPosition, center, RotationZeroAxis);
            
            if (_flipX)
            {
                angle = 360f - angle;
            }
            
            if (!IsAngleValid(angle)) return;
            
            var angleClamped = Mathf.Clamp(angle, _startAngle, _endAngle);

            if (_reverseValue)
            {
                angleClamped = _endAngle - angleClamped + _startAngle;
            }

            var colorValue = angleClamped.Remap01(_startAngle, _endAngle);
            
            Model.SetColorValue(colorValue, ColorValueType);
        }
        
        protected override ICursorUpdater GetCursorUpdater(float value)
        {
            var angle = Mathf.Clamp01(value).Remap(0f, 1f, _startAngle, _endAngle);
            
            if (_reverseValue)
            {
                angle = _endAngle - angle + _startAngle;
            }

            return new CursorRotationUpdater(angle, RotationAxis);
        }
        
        protected override void SetupMaterialOnValidated(Material material)
        {
            base.SetupMaterialOnValidated(material);

            _flipX = RotationAxis.z > 0;
            
            material.SetFloat(RotateProperty, _startAngle);
            material.SetFloat(SectorProperty, _endAngle - _startAngle);
            material.SetFloat(FlipXProperty, _flipX ? 1f : 0f);
            material.SetFloat(FlipYProperty, _flipY ? 1f : 0f);
        }
        
        private bool IsAngleValid(float angle)
        {
            return angle >= _startAngle - _clickAngleTolerance && angle <= _endAngle + _clickAngleTolerance;
        }
    }
}