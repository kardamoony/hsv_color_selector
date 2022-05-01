using MaterialController;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HSVColorSelector
{
    public abstract class ColorTargetBase : MonoBehaviour
    {
        [SerializeField] private PointerEventsProcessor _pointerEventsProcessor;

        private bool _initialized;
        private ColorSelectionModel _model;
        private IMaterialColorController _colorController;

        protected IMaterialColorController ColorController => _colorController ??= GetComponent<IMaterialColorController>();
        
        public void Initialize(ColorSelectionModel model)
        {
            _model = model;
            _initialized = model != null;
        }
        
        protected abstract void HandleOnPointerClick(PointerEventData pointerEventData);

        protected virtual void OnAwakened(){}

        protected Color ApplyColor()
        {
            if (_model == null) return Color.magenta;
            return _model.ApplyColor();
        }
        
        private void Awake()
        {
            _pointerEventsProcessor.OnPointerClickedEvent += HandleOnPointerClick;
            OnAwakened();
        }

        private void OnDestroy()
        {
            _pointerEventsProcessor.OnPointerClickedEvent -= HandleOnPointerClick;
        }
    }
}
