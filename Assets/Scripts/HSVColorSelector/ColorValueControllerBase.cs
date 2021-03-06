using MaterialController;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HSVColorSelector
{
    public abstract class ColorValueControllerBase : MonoBehaviour
    {
        [SerializeField] protected RectTransform RectTransform;
        [SerializeField] protected ColorValueType ColorValueType;
        [SerializeField] protected Transform Cursor;

        private bool _initialized;
        private PointerEventsProcessor _pointerEventsProcessor;
        private IMaterialProvider _materialProvider;
        
        protected ColorSelectionModel Model;

        private IMaterialProvider MaterialProvider
        {
            get
            {
                if (_materialProvider == null)
                {
                    _materialProvider = GetComponent<IMaterialProvider>();
                }

                return _materialProvider;
            }
        }

        public void Initialize(ColorSelectionModel model, PointerEventsProcessor pointerEventsProcessor)
        {
            if (_initialized) return;
            _pointerEventsProcessor = pointerEventsProcessor;
            Model = model;
            Subscribe(model, pointerEventsProcessor);
            HandleOnColorChanged(model);
            _initialized = true;
        }   

        public void Deinitialize()
        {
            if (!_initialized || Model == null) return;
            Unsubscribe(Model, _pointerEventsProcessor);
            Model = null;
            _initialized = false;
        }

        protected abstract void HandleOnColorChanged(ColorSelectionModel model);
        
        protected abstract void HandleOnPointerClickEvent(PointerEventData pointerEventData);

        protected virtual void HandleOnPointerDragEvent(PointerEventData pointerEventData, Vector2 dragStartPosition){}
        
        protected virtual void SetupMaterial(Material material){}

        private void Subscribe(ColorSelectionModel model, PointerEventsProcessor pointerEventsProcessor)
        {
            model.OnColorChanged += HandleOnColorChanged;
            pointerEventsProcessor.OnPointerClickedEvent += HandleOnPointerClickEvent;
            pointerEventsProcessor.OnPointerDragEvent += HandleOnPointerDragEvent;
        }

        private void Unsubscribe(ColorSelectionModel model, PointerEventsProcessor pointerEventsProcessor)
        {
            model.OnColorChanged -= HandleOnColorChanged;
            pointerEventsProcessor.OnPointerClickedEvent -= HandleOnPointerClickEvent;
            pointerEventsProcessor.OnPointerDragEvent -= HandleOnPointerDragEvent;
        }

        private void Awake()
        {
            SetupMaterial(MaterialProvider.Material);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            SetupMaterial(MaterialProvider.Material);
        }
#endif
    }
}