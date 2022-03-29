using System;
using ColorSelector.Components;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ColorSelector
{
    public class ColorSelector : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public event Action<Color> ColorChangedEvent; 

        [SerializeField] private ColorValueController[] _colorValueControllers;
        [SerializeField] private ColorSelectorComponent[] _colorSelectorComponents;
        
        [SerializeField] private Color _initialColor = Color.red;

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
            _currentSelection.SetColor(selectionType, value);
            UpdateColor();
        }

        private void OnColorChanged(Color color)
        {
            _currentSelection.SetColor(color);
            UpdateColor();
            
            foreach (var colorValueController in _colorValueControllers)
            {
                colorValueController.UpdateCursorPosition(_currentSelection);
            }
        }

        private void UpdateColor()
        {
            foreach (var component in _colorSelectorComponents)
            {
                component.OnColorChanged(_currentSelection);
            }
            
            ColorChangedEvent?.Invoke(_currentSelection.Color);
        }

        private void Awake()
        {
            OnColorChanged(_initialColor);
        }
        
#if UNITY_EDITOR
        
        [ContextMenu("Editor Set Color")]
        private void EditorSetColor()
        {
            OnColorChanged(_initialColor);
        }
#endif
        
    }
}
