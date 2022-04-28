using StaticHelpers;
using UnityEngine;

namespace Selector
{
    public class CircularColorSelectionParams
    {
        public Vector2 RotationZeroAxis;
        public Vector3 RotationAxis;
        public RectTransform RectTransform;
        
        public float DistanceTolerance;
    }
    
    public class CircularColorSelectionController : ColorSelectionController
    {
        protected RectTransform RectTransform { get; }
        protected Vector2 RotationZeroAxis { get; }
        protected Vector3 RotationAxis { get; }

        protected float DistanceTolerance { get; }

        public CircularColorSelectionController(ColorSelectionModel model, ColorSelectionModel.ColorValueType colorValueType, CircularColorSelectionParams @params) : base(model, colorValueType)
        {
            RectTransform = @params.RectTransform;
            RotationZeroAxis = @params.RotationZeroAxis;
            RotationAxis = @params.RotationAxis;
            DistanceTolerance = @params.DistanceTolerance;
        }
        
        public virtual ICursorUpdater GetCursorUpdater(float value)
        {
            var angle = Mathf.Clamp01(value).Remap(0f, 1f, 0f, 360f);
            return new CursorRotationUpdater(angle, RotationAxis);
        }

        public virtual void OnPointerEvent(Vector2 displayPosition, Vector2 validatePosition, float innerCircle, float outerCircle)
        {
            var (innerRadius, outerRadius) = GetRelativeCircleRadii(innerCircle, outerCircle);
            var center = RectTransform.position;

            if (!IsClickValid(center, validatePosition, innerRadius, outerRadius)) return;
            
            var angle = MathHelper.GetAngle360(displayPosition, center, RotationZeroAxis);
            var colorValue = ColorHelper.Angle360ToHue(angle);
            
            Model.UpdateColor(colorValue, ColorValueType);
        }

        protected bool IsClickValid(Vector3 center, Vector2 clickPosition, float innerRadius, float outerRadius)
        {
            var clickDistance = Vector2.Distance(center, clickPosition);
            return clickDistance >= (innerRadius - DistanceTolerance) && clickDistance <= (outerRadius + DistanceTolerance);
        }

        protected (float innerRadius, float outerRadius) GetRelativeCircleRadii(float innerCircle, float outerCircle)
        {
            var corners = new Vector3[4];
            RectTransform.GetWorldCorners(corners);
            var maxRadius = Vector3.Distance(corners[0], corners[1]) * 0.5f;
            
            var outerRadius = maxRadius * outerCircle;
            var innerRadius = maxRadius * innerCircle;

            var distance = innerRadius + (outerRadius - innerRadius) * 0.5f;

            return (innerRadius, outerRadius);
        }
    }
}
