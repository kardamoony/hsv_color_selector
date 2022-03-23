using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ColorPicker.Components
{
    public class ColorValueController : ColorSelectorComponent
    {
        protected static readonly int OuterCircle = Shader.PropertyToID("_OuterCircle");
        protected static readonly int InnerCircle = Shader.PropertyToID("_InnerCircle");

        [SerializeField] private RectTransform _rect;
        [SerializeField] private Image _img;
        [SerializeField] private Transform _cursorTransform;
        [SerializeField] private ColorSelection.SelectionType _selectionType;

        private readonly Vector3[] _corners = new Vector3[4];

        protected Vector3 RectCenter => _rect.position;
        protected Material Material => _img.material;
        
        public void ProcessClick(PointerEventData eventData, Vector3? dragStartPosition, Action<ColorSelection.SelectionType, float> callback)
        {
            var center = RectCenter;

            var isDragValid = IsClickValid(dragStartPosition, center, out _, out _);
            if (dragStartPosition.HasValue && !isDragValid) return;

            if (!IsClickValid(eventData.position, center, out var cursorPosition, out var colorValue) && !isDragValid) return;
            _cursorTransform.position = cursorPosition;
            callback?.Invoke(_selectionType, colorValue);
        }
        
        protected virtual bool IsClickValid(Vector3? position, Vector3 center, out Vector3 cursorPosition, out float colorValue)
        {
            cursorPosition = Vector3.zero;
            colorValue = 1f;
            return false;
        }

        protected virtual void OnMaterialSetup(Material material){}
        
        protected float GetMaxRadius(Vector3 center)
        {
            _rect.GetWorldCorners(_corners); //order: bottom left, top left, top right, bottom right
            return Vector3.Distance(center, new Vector3(center.x, _corners[0].y, center.z));
        }
        
        private void SetupMaterial()
        {
            if (!_img) return;
            OnMaterialSetup(_img.material);
        }

        private void Awake()
        {
            SetupMaterial();
        }

        private void OnValidate()
        {
            SetupMaterial();
        }
    }
}
