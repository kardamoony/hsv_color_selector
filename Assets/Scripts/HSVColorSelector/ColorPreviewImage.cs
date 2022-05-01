using UnityEngine;
using UnityEngine.UI;

namespace HSVColorSelector
{
    public class ColorPreviewImage : ColorPreviewBase
    {
        [SerializeField] private Image _image;
        
        public override void OnColorChanged(ColorSelectionModel model)
        {
            SetColor(model.GetColor(ColorValueType));
        }

        public override void SetColor(Color color)
        {
            _image.color = color;
        }
    }
}