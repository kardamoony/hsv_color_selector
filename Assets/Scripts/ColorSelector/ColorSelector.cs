using ColorPicker.Components;
using UnityEngine;
using UnityEngine.EventSystems;
namespace ColorPicker
{
    public class ColorSelector : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private ColorValueController[] _colorValueControllers;
        [SerializeField] private ColorSelectorComponent[] _colorSelectorComponents;

        private readonly ColorSelection _currentSelection = new ColorSelection(1f, 1f, 1f);
        private bool _isDragging;
        private Vector3 _dragStartPosition;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isDragging) return;
            ProcessPointerEvent(eventData);
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDragging = true;
            _dragStartPosition = eventData.position;
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            ProcessPointerEvent(eventData, _dragStartPosition);
        }

        private void ProcessPointerEvent(PointerEventData eventData, Vector3? dragStartPosition = null)
        {
            foreach (var valueController in _colorValueControllers)
            {
                valueController.ProcessClick(eventData, dragStartPosition, OnColorChanged);
            }
        }

        private void OnColorChanged(ColorSelection.SelectionType selectionType, float value)
        {
            _currentSelection.SetColorComponent(selectionType, value);
            foreach (var component in _colorSelectorComponents)
            {
                component.OnColorChanged(_currentSelection);
            }
        }
    }
}
