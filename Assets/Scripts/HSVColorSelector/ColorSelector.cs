using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;

namespace HSVColorSelector
{
    public class ColorSelector : MonoBehaviour
    {
        public event Action<Color> OnColorChanged;

        [SerializeField] private PointerEventsProcessor _pointerEventsProcessor;
        [SerializeField] private ColorValueControllerBase[] _controllers;
        [SerializeField] private List<ColorPreviewBase> _colorPreviews;
        
        [Space(10f)]
        [SerializeField] private Color _initialColor = Color.red;

        private ColorSelectionModel _model;
        private ColorSelectionModel Model => _model ??= CreateModel();

        public void AddColorListener(ColorPreviewBase colorPreview)
        {
            _colorPreviews ??= new List<ColorPreviewBase>();
            if (_colorPreviews.Contains(colorPreview)) return;
            _colorPreviews.Add(colorPreview);
        }

        public void RemoveColorListener(ColorPreviewBase colorPreview)
        {
            if (_colorPreviews == null || !_colorPreviews.Contains(colorPreview)) return;
            _colorPreviews.Remove(colorPreview);
        }

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

        private void HandleOnColorChanged(ColorSelectionModel model)
        {
            foreach (var preview in _colorPreviews)
            {
                preview.OnColorChanged(model);
            }
            
            OnColorChanged?.Invoke(model.GetColor(ColorValueType.RGBA));
        }

        private void Awake()
        {
            InitializeControllers(Model);
            InitializePreviews(Model);
            Model.OnColorChanged += HandleOnColorChanged;
        }

        private void OnDestroy()
        {
            DeinitializeControllers();
            Model.OnColorChanged -= HandleOnColorChanged;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            CollectViews();
            CollectPreviews();
        }

        [ContextMenu("Collect Views")]
        private void CollectViews()
        {
            _controllers = GetComponentsInChildren<ColorValueControllerBase>();
        }

        [ContextMenu("Collect Color Previews")]
        private void CollectPreviews()
        {
            _colorPreviews = GetComponentsInChildren<ColorPreviewBase>().ToList();
        }
#endif
    }
}
