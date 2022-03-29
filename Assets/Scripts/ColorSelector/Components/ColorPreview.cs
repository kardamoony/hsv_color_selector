using UnityEngine;
using UnityEngine.UI;

namespace ColorSelector.Components
{
    public class ColorPreview : ColorSelectorComponent
    {
        [SerializeField] protected Image PreviewImg;
        [SerializeField] protected ColorSelection.SelectionType PreviewType = ColorSelection.SelectionType.Color;

        public override void OnColorChanged(ColorSelection colorSelection)
        {
            SetPreviewColor(colorSelection.GetColor(PreviewType));
        }
        
        protected virtual void SetPreviewColor(Color color)
        {
            if (PreviewImg) PreviewImg.color = color;
        }
    }
}
