using System;
using UnityEngine;

namespace HSVColorSelector
{
    public enum ColorValueType
    {
        RGBA,
        Hue,
        Saturation,
        Value
    }
    
    public class ColorSelectionModel
    {
        public event Action<ColorSelectionModel> OnColorChanged;

        private Color _rgbaColor;
        private Color _hueColor;
        private Color _valueColor;
        private Color _saturationColor;

        private float _h;
        private float _s;
        private float _v;

        public ColorSelectionModel(Color initialColor)
        {
            _rgbaColor = initialColor;
            Color.RGBToHSV(initialColor, out _h, out _s, out _v);
            _hueColor = Color.HSVToRGB(_h, 1f, 1f);
            _saturationColor = Color.HSVToRGB(_h, _s, 1f);
            _valueColor = Color.HSVToRGB(_h, 1f, _v);
        }

        public void UpdateColor(float value, ColorValueType type)
        {
            _rgbaColor = UpdateColorInternal(value, type);
            OnColorChanged?.Invoke(this);
        }

        public Color GetColor(ColorValueType colorValueType)
        {
            switch (colorValueType)
            {
                case ColorValueType.Hue : return _hueColor;
                case ColorValueType.Saturation : return _saturationColor;
                case ColorValueType.Value : return _valueColor;
                default: return _rgbaColor;
            }
        }

        public float GetColorValue(ColorValueType colorValueType)
        {
            switch (colorValueType)
            {
                case ColorValueType.Hue : return _h;
                case ColorValueType.Saturation : return _s;
                case ColorValueType.Value : return _v;
                default: return 0f;
            }
        }

        private Color UpdateColorInternal(float value, ColorValueType type)
        {
            switch (type)
            {
                case ColorValueType.Hue:
                {
                    _h = value;
                    break;
                }
                case ColorValueType.Saturation:
                {
                    _s = value;
                    break;
                }
                case ColorValueType.Value:
                {
                    _v = value;
                    break;
                }
            }
            
            _hueColor = Color.HSVToRGB(_h, 1f, 1f);
            _saturationColor = Color.HSVToRGB(_h, _s, 1f);
            _valueColor = Color.HSVToRGB(_h, 1f, _v);
            
            return Color.HSVToRGB(_h, _s, _v);
        }
    }
}
