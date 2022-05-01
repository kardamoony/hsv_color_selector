using UI;
using UnityEngine;

namespace HSVColorSelector
{
    public class ColorSelector : MonoBehaviour
    {
        [SerializeField] private PointerEventsProcessor _pointerEventsProcessor;
        
        [Space(5f)] 
        [SerializeField] private ColorValueControllerBase[] _controllers;
        [SerializeField] private ColorPreviewBase[] _colorPreviews;

        [Space(5f)] 
        [SerializeField] private ColorTargetBase[] _colorTargets;

        [Space(5f)] 
        [SerializeField] private ColorHistory _colorHistory;
        
        [Space(10f)]
        [SerializeField] private Color _initialColor = Color.red;

        private ColorSelectionModel _model;
        private ColorSelectionModel Model => _model ??= CreateModel();
        
        private ColorSelectionModel CreateModel()
        {
            return new ColorSelectionModel(_initialColor);
        }
        
        private void InitializeControllers(ColorSelectionModel model)
        {
            foreach (var controller in _controllers)
            {
                controller.Initialize(model, _pointerEventsProcessor);
            }
        }

        private void DeinitializeControllers()
        {
            foreach (var controller in _controllers)
            {
                controller.Deinitialize();
            }
        }

        private void InitializePreviews(ColorSelectionModel model)
        {
            foreach (var preview in _colorPreviews)
            {
                preview.OnColorChanged(model);
            }
        }

        private void InitializeColorTargets(ColorSelectionModel model)
        {
            foreach (var target in _colorTargets)
            {
                target.Initialize(model);
            }
        }

        private void InitializeColorHistory(ColorSelectionModel model)
        {
            _colorHistory.Initialize(model);
        }
        
        private void DeinitializeColorHistory()
        {
            _colorHistory.Deinitialize();
        }

        private void HandleOnColorChanged(ColorSelectionModel model)
        {
            foreach (var preview in _colorPreviews)
            {
                preview.OnColorChanged(model);
            }
        }

        private void HandleOnColorApplied(Color color)
        {
            
        }

        private void Awake()
        {
            InitializeControllers(Model);
            InitializePreviews(Model);
            InitializeColorTargets(Model);
            InitializeColorHistory(Model);
            Model.OnColorChanged += HandleOnColorChanged;
            Model.OnColorApplied += HandleOnColorApplied;
        }

        private void OnDestroy()
        {
            DeinitializeControllers();
            DeinitializeColorHistory();
            Model.OnColorChanged -= HandleOnColorChanged;
            Model.OnColorApplied -= HandleOnColorApplied;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            CollectColorViews();
            CollectColorPreviews();
            CollectColorTargets();
        }

        [ContextMenu("Collect Views")]
        private void CollectColorViews()
        {
            _controllers = GetComponentsInChildren<ColorValueControllerBase>();
        }

        [ContextMenu("Collect Color Previews")]
        private void CollectColorPreviews()
        {
            _colorPreviews = GetComponentsInChildren<ColorPreviewBase>();
        }

        [ContextMenu("Collect Color Targets")]
        private void CollectColorTargets()
        {
            _colorTargets = FindObjectsOfType<ColorTargetBase>();
        }
#endif
    }
}
