using UnityEngine;
using UnityEngine.UI;

namespace ColorSelector
{
    public class ImageColorPreview : ColorPreviewBase
    {
        [SerializeField] private Image _image;
        
        public override void OnColorChanged(ColorSelectionModel model)
        {
            _image.color = model.GetColor(ColorValueType);
        }
    }
}
