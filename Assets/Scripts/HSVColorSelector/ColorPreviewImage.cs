using UnityEngine;
using UnityEngine.UI;

namespace HSVColorSelector
{
    public class ColorPreviewImage : ColorPreviewBase
    {
        [SerializeField] private Image _image;
        
        public override void OnColorChanged(ColorSelectionModel model)
        {
            _image.color = model.GetColor(ColorValueType);
        }
    }
}