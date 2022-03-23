using UnityEngine;

namespace ColorPicker.Components
{
    public class MaterialColorPreview : ColorPreview
    {
        private static readonly int SamplerProperty = Shader.PropertyToID("_SamplerColor");
        
        protected override void SetPreviewColor(Color color)
        {
            if (PreviewImg) PreviewImg.material.SetColor(SamplerProperty, color);
        }
    }
}
