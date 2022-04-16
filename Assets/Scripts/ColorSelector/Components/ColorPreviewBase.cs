using UnityEngine;

namespace ColorSelector.Components
{
    public abstract class ColorPreviewBase : ColorSelectorComponentBase
    {
        [SerializeField] protected ColorSelection.SelectionType PreviewType = ColorSelection.SelectionType.Color;
        
        public override void UpdateViewOnColorChanged(ColorSelection colorSelection)
        {
            SetPreviewColor(colorSelection.GetColor(PreviewType));
        }

        protected abstract void SetPreviewColor(Color color);
    }
}
