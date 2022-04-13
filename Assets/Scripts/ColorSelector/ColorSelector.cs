using System;
using ColorSelector.Components;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ColorSelector
{
    [RequireComponent(typeof(PointerEventsProcessor))]
    public class ColorSelector : MonoBehaviour
    {
        [SerializeField] private PointerEventsProcessor _pointerEventsProcessor;
        
        [Space(5f)]
        [SerializeField] private ColorSelectorComponent[] _colorSelectorComponents;
        [SerializeField] private ColorValueController[] _colorValueControllers;
        
        [Space(5f)]
        [SerializeField] private Color _initialColor = Color.red;
        
        private readonly ColorSelection _currentSelection = new ColorSelection(1f, 1f, 1f);
        
        private void HandlePointerClickEvent(PointerEventData pointerEventData)
        {
            HandlePointerEvent(pointerEventData.position);
        }

        private void HandlePointerDragEvent(PointerEventData pointerEventData, Vector2 dragStartPosition)
        {
            HandlePointerEvent(pointerEventData.position, dragStartPosition);
        }

        private void HandlePointerEvent(Vector3 pointerPosition, Vector3? dragStartPosition = null)
        {
            foreach (var valueController in _colorValueControllers)
            {
                valueController.ProcessClick(pointerPosition, dragStartPosition, HandleColorChanged);
            }
        }

        private void HandleColorChanged(ColorSelection.SelectionType selectionType, float value)
        {
            _currentSelection.SetColor(selectionType, value);
            NotifyListeners(_currentSelection);
        }

        private void HandleColorChanged(Color color)
        {
            var colorSelection = _currentSelection.SetColor(color);
            NotifyListeners(colorSelection);

        }

        private void NotifyListeners(ColorSelection colorSelection)
        {
            foreach (var component in _colorSelectorComponents)
            {
                component.OnColorChanged(colorSelection);
            }
        }

        private void Subscribe(PointerEventsProcessor pointerEventsProcessor)
        {
            if (!pointerEventsProcessor) return;
            pointerEventsProcessor.OnPointerClickedEvent += HandlePointerClickEvent;
            pointerEventsProcessor.OnPointerDragEvent += HandlePointerDragEvent;
        }

        private void Unsubscribe(PointerEventsProcessor pointerEventsProcessor)
        {
            if (!pointerEventsProcessor) return;
            pointerEventsProcessor.OnPointerClickedEvent -= HandlePointerClickEvent;
            pointerEventsProcessor.OnPointerDragEvent -= HandlePointerDragEvent;
        }

        private void Awake()
        {
            _pointerEventsProcessor = GetComponent<PointerEventsProcessor>();
            Subscribe(_pointerEventsProcessor);
            HandleColorChanged(_initialColor);
        }

        private void OnDestroy()
        {
            Unsubscribe(_pointerEventsProcessor);
        }

#if UNITY_EDITOR
        
        [ContextMenu("Editor Set Color")]
        private void EditorSetColor()
        {
            HandleColorChanged(_initialColor);
        }

        private void Reset()
        {
            _colorSelectorComponents = GetComponentsInChildren<ColorSelectorComponent>();
            _colorValueControllers = GetComponentsInChildren<ColorValueController>();
            _initialColor = Color.white;
        }
#endif
        
    }
}
