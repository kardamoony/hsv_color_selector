using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ColorSelector.Components
{
    public class ColorValueController : ColorSelectorComponent
    {
        [SerializeField] private RectTransform _rect;
        [SerializeField] private ColorCursor _cursor;
        [SerializeField] private ColorSelection.SelectionType _selectionType;
        
        [SerializeField] protected Material Material;

        private readonly Vector3[] _corners = new Vector3[4];
        
        public void UpdateCursorPosition(ColorSelection colorSelection)
        {
            _cursor.UpdatePosition(ValueToCursorData(_rect.position, colorSelection.GetSelectionValue(_selectionType)));
        }
        
        public void ProcessClick(PointerEventData eventData, Vector3? dragStartPosition, Action<ColorSelection.SelectionType, float> callback)
        {
            var center = _rect.position;

            var isDragValid = IsClickValid(dragStartPosition, center, out _);
            if (dragStartPosition.HasValue && !isDragValid) return;

            if (!IsClickValid(eventData.position, center, out var data) && !isDragValid) return;
            _cursor.UpdatePosition(data);
            callback?.Invoke(_selectionType, data.Value);
        }

        protected virtual bool IsClickValid(Vector3? position, Vector3 center, out ColorCursorData data)
        {
            data = default;
            return false;
        }

        protected virtual ColorCursorData ValueToCursorData(Vector3 center, float value)
        {
            return default;
        }

        protected virtual void OnMaterialSetup(Material material){}
        
        protected float GetMaxRadius(Vector3 center)
        {
            //TODO: world corners are bad!!!!
            _rect.GetWorldCorners(_corners); //order is: bottom left, top left, top right, bottom right
            return Vector3.Distance(center, new Vector3(center.x, _corners[0].y, center.z));
        }
        
        private void SetupMaterial()
        {
            if (!Material) return;
            OnMaterialSetup(Material);
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
