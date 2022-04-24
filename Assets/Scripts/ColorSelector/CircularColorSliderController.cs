using StaticHelpers;
using UnityEngine;

namespace ColorSelector
{
    public class CircularColorSliderParams : CircularColorSelectionParams
    {
        public float StartAngle;
        public float EndAngle;
        public float AngleTolerance;
        
        public bool ReverseValue;
        public bool FlipX;
    }
    
    public class CircularColorSliderController : CircularColorSelectionController
    {
        private readonly float _startAngle;
        private readonly float _endAngle;
        private readonly float _angleTolerance;
        
        private readonly bool _reverseValue;

        private readonly bool _flipX; 
        
        public CircularColorSliderController(ColorSelectionModel model, ColorSelectionModel.ColorValueType colorValueType, CircularColorSelectionParams @params) : base(model, colorValueType, @params)
        {
            if (!(@params is CircularColorSliderParams sliderParams)) return;
            _startAngle = sliderParams.StartAngle;
            _endAngle = sliderParams.EndAngle;
            _reverseValue = sliderParams.ReverseValue;
            _angleTolerance = sliderParams.AngleTolerance;
            _flipX = sliderParams.FlipX;
        }
        
        public override void OnPointerEvent(Vector2 displayPosition, Vector2 validatePosition, float innerCircle, float outerCircle)
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
            
            Model.UpdateColor(colorValue, ColorValueType);
        }

        public override ICursorUpdater GetCursorUpdater(float value)
        {
            var angle = Mathf.Clamp01(value).Remap(0f, 1f, _startAngle, _endAngle);
            
            if (_reverseValue)
            {
                angle = _endAngle - angle + _startAngle;
            }

            return new CursorRotationUpdater(angle, RotationAxis);
        }

        private bool IsAngleValid(float angle)
        {
            return angle >= _startAngle - _angleTolerance && angle <= _endAngle + _angleTolerance;
        }
    }
}
