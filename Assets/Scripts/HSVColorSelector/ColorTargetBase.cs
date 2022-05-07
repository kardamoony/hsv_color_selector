using MaterialController;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HSVColorSelector
{
    public abstract class ColorTargetBase : MonoBehaviour
    {
        [SerializeField] private PointerEventsProcessor _pointerEventsProcessor;
        
        private ColorSelectionModel _model;
        private IMaterialColorController _colorController;

        protected IMaterialColorController ColorController => _colorController ??= GetComponent<IMaterialColorController>();
        
        public void Initialize(ColorSelectionModel model)
        {
            _model = model;
        }
        
        protected abstract void HandleOnPointerClick(PointerEventData pointerEventData);
        

        protected Color ApplyColor()
        {
            if (_model == null) return Color.black;
            return _model.ApplyColor();
        }
        
        private void Awake()
        {
            _pointerEventsProcessor.OnPointerClickedEvent += HandleOnPointerClick;
        }

        private void OnDestroy()
        {
            _pointerEventsProcessor.OnPointerClickedEvent -= HandleOnPointerClick;
        }
        
#if UNITY_EDITOR
        private void Reset()
        {
            _pointerEventsProcessor = GetComponent<PointerEventsProcessor>();
        }
#endif
    }
}
