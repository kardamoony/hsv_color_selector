using StaticHelpers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HSVColorSelector
{
    public class RingColorValueController : ColorValueControllerBase
    {
        private static readonly int InnerCircleProperty = Shader.PropertyToID("_InnerCircle");
        private static readonly int OuterCircleProperty = Shader.PropertyToID("_OuterCircle");
        
        [Space(10f)]
        [SerializeField] protected float ClickDistanceTolerance = 15f;
        
        [SerializeField] protected Vector2 RotationZeroAxis = Vector2.up;
        [SerializeField] protected Vector3 RotationAxis = Vector3.back;
        
        [Range(0, 1)]
        [SerializeField] protected float InnerCircle;
        [Range(0, 1)] 
        [SerializeField] protected float OuterCircle;
        
        protected virtual ICursorUpdater GetCursorUpdater(float value)
        {
            var angle = Mathf.Clamp01(value).Remap(0f, 1f, 0f, 360f);
            return new CursorRotationUpdater(angle, RotationAxis);
        }
        
        protected virtual void OnPointerEvent(Vector2 displayPosition, Vector2 validatePosition, float innerCircle, float outerCircle)
        {
            var (innerRadius, outerRadius) = GetRelativeCircleRadii(innerCircle, outerCircle);
            var center = RectTransform.position;

            if (!IsClickValid(center, validatePosition, innerRadius, outerRadius)) return;
            
            var angle = MathHelper.GetAngle360(displayPosition, center, RotationZeroAxis);
            var colorValue = ColorHelper.Angle360ToHue(angle);
            
            Model.SetColorValue(colorValue, ColorValueType);
        }
        
        protected override void HandleOnColorChanged(ColorSelectionModel model)
        {
            var value = model.GetColorValue(ColorValueType);
            var cursorUpdater = GetCursorUpdater(value);
            cursorUpdater.UpdateCursor(Cursor);
        }

        protected override void HandleOnPointerClickEvent(PointerEventData pointerEventData)
        {
            OnPointerEvent(pointerEventData.position, pointerEventData.position, InnerCircle, OuterCircle);
        }

        protected override void HandleOnPointerDragEvent(PointerEventData pointerEventData, Vector2 dragStartPositon)
        {
            OnPointerEvent(pointerEventData.position, dragStartPositon, InnerCircle, OuterCircle);
        }

        protected override void SetupMaterial(Material material)
        {
            material.SetFloat(InnerCircleProperty, InnerCircle);
            material.SetFloat(OuterCircleProperty, OuterCircle);
        }

        protected bool IsClickValid(Vector3 center, Vector2 clickPosition, float innerRadius, float outerRadius)
        {
            var clickDistance = Vector2.Distance(center, clickPosition);
            return clickDistance >= innerRadius - ClickDistanceTolerance 
                   && clickDistance <= outerRadius + ClickDistanceTolerance;
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