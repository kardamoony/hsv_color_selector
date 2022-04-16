using UnityEngine;
using UnityEngine.UI;

namespace ColorSelector.Components
{
    public class ColorPreviewImage : ColorPreviewBase
    {
        [SerializeField] private Image _previewImg;

        protected override void SetPreviewColor(Color color)
        {
            if (_previewImg) _previewImg.color = color;
        }
        
#if UNITY_EDITOR
        private void Reset()
        {
            _previewImg = GetComponent<Image>();
        }
#endif
    }
}
