using System.Collections.Generic;
using Commands;
using StaticHelpers;
using UnityEngine;
using UnityEngine.UI;

namespace HSVColorSelector
{
    public class ColorHistory : MonoBehaviour
    {
        [SerializeField] private ColorSwatch _swatchPrefab;
        [SerializeField] private Transform _swatchesParent;
        [SerializeField] private int _historyElementsCount = 5;
        [SerializeField] private Button _undoButton;

        private ColorSelectionModel _model;
        private ColorSwatch[] _swatches;
        private readonly List<ColorApplyCommand> _colorCommands = new List<ColorApplyCommand>();

        public void Initialize(ColorSelectionModel model)
        {
            _model = model;
            InitializeSwatches();
            UpdateUndoButtonVisibility(0);
            if (_model != null) _model.OnColorApplied += HandleOnColorApplied;
            if (_undoButton) _undoButton.onClick.AddListener(HandleUndoButtonClicked);
        }
        
        public void Deinitialize()
        {
            DeinitializeSwatches();
            if (_model != null) _model.OnColorApplied -= HandleOnColorApplied;
            if (_undoButton) _undoButton.onClick.RemoveListener(HandleUndoButtonClicked);
        }

        private void InitializeSwatches()
        {
            _swatches = new ColorSwatch[_historyElementsCount];

            for (var i = 0; i < _historyElementsCount; i++)
            {
                var swatch = Instantiate(_swatchPrefab, _swatchesParent);
                swatch.Initialize();
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

        private void HandleOnColorApplied(ColorApplyCommand command)
        {
            if (HasSimilarColorInHistory(command.Color)) return;
            UpdateColorsHistory(command, out var colorsCount);
            UpdateSwatches(colorsCount);
            UpdateUndoButtonVisibility(colorsCount);
        }

        private bool HasSimilarColorInHistory(Color color)
        {
            return _colorCommands.Exists(c => c.Color.Approximately(color));
        }

        private void HandleOnSwatchClicked(Color swatchColor)
        {
            _model.SetColor(swatchColor);
        }

        private void HandleUndoButtonClicked()
        {
            var colorsCount = _colorCommands.Count;
            if (colorsCount < 1) return;
            var command = _colorCommands[0];
            _colorCommands.Remove(command);
            command.Undo();

            var newColorsCount = colorsCount - 1;
            
            UpdateSwatches(newColorsCount);
            UpdateUndoButtonVisibility(newColorsCount);
        }

        private void UpdateColorsHistory(ColorApplyCommand command, out int newColorsCount)
        {
            var colorsCount = _colorCommands.Count;

            if (colorsCount >= _historyElementsCount)
            {
                _colorCommands.RemoveAt(colorsCount - 1);
            }
            
            _colorCommands.Insert(0, command);
            newColorsCount = _colorCommands.Count;
        }

        private void UpdateSwatches(int colorsCount)
        {
            for (var i = 0; i < _historyElementsCount; i++)
            {
                var swatch = _swatches[i];

                if (i < colorsCount)
                {
                    swatch.Activate(_colorCommands[i].Color);
                }
                else
                {
                    swatch.Deactivate();
                }
            }
        }

        private void UpdateUndoButtonVisibility(int colorsCount)
        {
            if (!_undoButton) return;
            _undoButton.gameObject.SetActive(colorsCount > 0);
        }
    }
}
