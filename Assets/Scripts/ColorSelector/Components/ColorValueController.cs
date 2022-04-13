using System;
using UnityEngine;

namespace ColorSelector.Components
{
    public abstract class ColorValueController : ColorSelectorComponent
    {
        [SerializeField] private RectTransform _rect;
        [SerializeField] private ColorSelection.SelectionType _selectionType;
        [SerializeField]private Transform _cursorTransform;
        
        [SerializeField] protected Material Material;

        private readonly Vector3[] _corners = new Vector3[4];
        
        public override void OnColorChanged(ColorSelection colorSelection)
        {
            UpdateCursor(ValueToCursorInfo(_rect.position, colorSelection.GetSelectionValue(_selectionType)));
        }

        public void ProcessClick(Vector3 pointerPosition, Vector3? dragStartPosition, Action<ColorSelection.SelectionType, float> callback)
        {
            var center = _rect.position;

            var isDragValid = IsClickValid(dragStartPosition, center, out _, out _);
            if (dragStartPosition.HasValue && !isDragValid) return;

            if (!IsClickValid(pointerPosition, center, out var cursorPositionInfo, out var value) && !isDragValid) return;
  
            //UpdateCursor(cursorPositionInfo);
 
            callback?.Invoke(_selectionType, value);
        }

        protected abstract bool IsClickValid(Vector3? position, Vector3 center, out ICursorInfo positionInfo, out float value);

        protected abstract ICursorInfo ValueToCursorInfo(Vector3 center, float value);

        protected abstract void OnMaterialSetup(Material material);
        
        protected float GetMaxRadius(Vector3 center)
        {
            //TODO: world corners are bad!!!!
            _rect.GetWorldCorners(_corners); //order is: bottom left, top left, top right, bottom right
            return Vector3.Distance(center, new Vector3(center.x, _corners[0].y, center.z));
        }

        private void UpdateCursor(ICursorInfo cursorInfo)
        {
            if (!_cursorTransform) return;
            cursorInfo.UpdateCursor(in _cursorTransform);
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
