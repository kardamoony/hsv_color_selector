using System.Collections.Generic;
using StaticHelpers;
using UnityEngine;

namespace HSVColorSelector
{
    public class ColorHistory : MonoBehaviour
    {
        [SerializeField] private ColorSwatch _swatchPrefab;
        [SerializeField] private Transform _swatchesParent;
        [SerializeField] private int _historyElementsCount = 5;

        private ColorSelectionModel _model;
        private ColorSwatch[] _swatches;
        private readonly List<Color> _colors = new List<Color>();

        public void Initialize(ColorSelectionModel model)
        {
            _model = model;
            InitializeSwatches();
            if (_model != null) _model.OnColorApplied += HandleOnColorApplied;
        }
        
        public void Deinitialize()
        {
            DeinitializeSwatches();
            if (_model != null) _model.OnColorApplied -= HandleOnColorApplied;
        }

        private void InitializeSwatches()
        {
            _swatches = new ColorSwatch[_historyElementsCount];

            for (var i = 0; i < _historyElementsCount; i++)
            {
                var swatch = Instantiate(_swatchPrefab, _swatchesParent);
                swatch.Deactivate();
                swatch.OnClicked += HandleOnSwatchClicked;
                _swatches[i] = swatch;
            }
        }

        private void DeinitializeSwatches()
        {
            foreach (var swatch in _swatches)
            {
                swatch.Deactivate();
                swatch.OnClicked -= HandleOnSwatchClicked;
            }
        }

        private void HandleOnColorApplied(Color newColor)
        {
            if (HasSimilarColorInHistory(newColor)) return;
            UpdateColorsHistory(newColor, out var colorsCount);
            UpdateSwatches(colorsCount);
        }

        private bool HasSimilarColorInHistory(Color color)
        {
            return _colors.Exists(c => c.Approximately(color));
        }

        private void HandleOnSwatchClicked(Color swatchColor)
        {
            _model.SetColor(swatchColor);
        }

        private void UpdateColorsHistory(Color newColor, out int newColorsCount)
        {
            var colorsCount = _colors.Count;

            if (colorsCount >= _historyElementsCount)
            {
                _colors.RemoveAt(colorsCount - 1);
            }
            
            _colors.Insert(0, newColor);
            newColorsCount = _colors.Count;
        }

        private void UpdateSwatches(int colorsCount)
        {
            for (var i = 0; i < _historyElementsCount; i++)
            {
                var swatch = _swatches[i];

                if (i < colorsCount)
                {
                    swatch.Activate(_colors[i]);
                }
                else
                {
                    swatch.Deactivate();
                }
            }
        }
    }
}
