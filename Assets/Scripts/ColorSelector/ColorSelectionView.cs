using MaterialController;
using MVC;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Selector
{
    public abstract class ColorSelectionView : ViewBase<ColorSelectionModel>
    {
        [SerializeField] private RectTransform _rect;
        [SerializeField] private Transform _cursor;
        [SerializeField] private PointerEventsProcessor _pointerEventsProcessor;

        private IMaterialProvider _materialProvider;
        protected RectTransform RectTransform => _rect;
        protected Transform Cursor => _cursor;

        protected override void OnInitialized(ColorSelectionModel model)
        {
            HandleOnColorChanged(model);
        }

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
        
        protected virtual void UpdateMaterialOnValidated(Material material) {}

        protected abstract void HandleOnColorChanged(ColorSelectionModel model);
        
        protected override void SubscribeToModel(ColorSelectionModel model)
        {
            model.OnColorChanged += HandleOnColorChanged;
        }

        protected override void UnSubscribeFromModel(ColorSelectionModel model)
        {
            model.OnColorChanged -= HandleOnColorChanged;
        }
        
        protected virtual void HandlePointerClickEvent(PointerEventData pointerEventData){}

        protected virtual void HandlePointerDragEvent(PointerEventData pointerEventData, Vector2 dragStartPosition){}

        private void OnValidate()
        {
            _materialProvider ??= GetComponent<IMaterialProvider>();
            if (_materialProvider == null) return;
            UpdateMaterialOnValidated(_materialProvider.Material);
        }
    }
}
