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
        
        public abstract void ApplyColor(Color color);
        
        public void Initialize(ColorSelectionModel model)
        {
            _model = model;
        }
        
        private void HandleOnPointerClick(PointerEventData pointerEventData)
        {
            _model?.ApplyColor(this, ColorController.GetColor());
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
