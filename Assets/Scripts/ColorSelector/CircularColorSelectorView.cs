using UnityEngine;
using UnityEngine.EventSystems;

namespace ColorSelector
{
    public class CircularColorSelectorView : ColorSelectionView
    {
        private static readonly int InnerCircleProperty = Shader.PropertyToID("_InnerCircle");
        private static readonly int OuterCircleProperty = Shader.PropertyToID("_OuterCircle");
        
        [SerializeField] protected ColorSelectionModel.ColorValueType _colorValueType;
        
        [Space(10f)]
        [SerializeField] protected float ClickDistanceTolerance = 15f;
        
        [SerializeField] protected Vector2 ZeroRotation = Vector2.up;
        [SerializeField] protected Vector3 RotationAxis = Vector3.back;
        
        [Range(0, 1)]
        [SerializeField] protected float InnerCircle;
        [Range(0, 1)] 
        [SerializeField] protected float OuterCircle;

        protected CircularColorSelectionController Controller;
        protected ColorSelectionModel.ColorValueType ColorValueType => _colorValueType;

        protected override void CreateControllers(ColorSelectionModel model)
        {
            var @params = new CircularColorSelectionParams
            {
                RectTransform = RectTransform,
                RotationAxis = RotationAxis,
                RotationZeroAxis = ZeroRotation,
                DistanceTolerance = ClickDistanceTolerance
            };
            
            Controller = new CircularColorSelectionController(model, ColorValueType, @params);
        }

        protected override void HandleOnColorChanged(ColorSelectionModel model)
        {
            var value = model.GetColorValue(ColorValueType);
            var cursorUpdater = Controller.GetCursorUpdater(value);
            cursorUpdater.UpdateCursor(Cursor);
        }
        
        protected override void UpdateMaterialOnValidated(Material material)
        {
            material.SetFloat(InnerCircleProperty, InnerCircle);
            material.SetFloat(OuterCircleProperty, OuterCircle);
        }

        protected override void HandlePointerClickEvent(PointerEventData pointerEventData)
        {
            Controller.OnPointerEvent(pointerEventData.position, pointerEventData.position, InnerCircle, OuterCircle);
        }

        protected override void HandlePointerDragEvent(PointerEventData pointerEventData, Vector2 dragStartPosition)
        {
            Controller.OnPointerEvent(pointerEventData.position, dragStartPosition, InnerCircle, OuterCircle);
        }
    }
}
