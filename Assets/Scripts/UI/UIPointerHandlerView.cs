using ColorSelector;
using MVC;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public abstract class UIPointerHandlerView<TModel> : ViewBase<TModel> where TModel : ModelBase
    {
        [SerializeField] private PointerEventsProcessor _pointerEventsProcessor;

        protected abstract bool ValidateClickEvent(Vector2 clickPosition, out ICursorUpdater validatedEvent);
        protected abstract bool ValidateDragEvent(Vector2 dragCurrentPosition, Vector2 dragStartPosition, out ICursorUpdater validatedEvent);
        protected abstract void OnPointerEventValidated(ICursorUpdater pointerEventValidated);

        protected override void OnSubscribed()
        {
            _pointerEventsProcessor.OnPointerClickedEvent += HandlePointerClickEvent;
            _pointerEventsProcessor.OnPointerDragEvent += HandlePointerDragEvent;
        }

        protected override void OnUnSubscribed()
        {
            _pointerEventsProcessor.OnPointerClickedEvent -= HandlePointerClickEvent;
            _pointerEventsProcessor.OnPointerDragEvent -= HandlePointerDragEvent;
        }

        private void HandlePointerClickEvent(PointerEventData pointerEventData)
        {
            if (ValidateClickEvent(pointerEventData.position, out var pointerEventValidated))
            {
                OnPointerEventValidated(pointerEventValidated);
            }
        }

        private void HandlePointerDragEvent(PointerEventData pointerEventData, Vector2 dragStartPosition)
        {
            if (ValidateDragEvent(pointerEventData.position, dragStartPosition, out var pointerEventValidated))
            {
                OnPointerEventValidated(pointerEventValidated);
            }
        }
    }

}
