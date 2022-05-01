using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class PointerEventsProcessor : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public delegate void PointerClickEventHandler(PointerEventData pointerEventData);
        public delegate void PointerDragEventHandler(PointerEventData pointerEventData, Vector2 dragStartPosition);

        public event PointerClickEventHandler OnPointerClickedEvent;
        public event PointerDragEventHandler OnPointerDragEvent;

        [SerializeField] private bool _debug;
        
        private bool _isDragging;
        private Vector2 _dragStartPosition;
    
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isDragging) return;
            InvokePointerClickEvent(eventData);
            ShowDebugInfo(eventData);
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
            InvokePointerDragEvent(eventData, _dragStartPosition);
        }

        private void InvokePointerClickEvent(PointerEventData eventData)
        {
            OnPointerClickedEvent?.Invoke(eventData);
        }

        private void InvokePointerDragEvent(PointerEventData eventData, Vector2 dragStartPosition)
        {
            OnPointerDragEvent?.Invoke(eventData, dragStartPosition);
        }

        private void ShowDebugInfo(PointerEventData eventData)
        {
            if (!_debug) return;
            Debug.Log($"{GetType().Name}: click={eventData.pointerClick.name}");
        }

    }
}
