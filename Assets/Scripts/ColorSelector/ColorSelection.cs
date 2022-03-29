using UnityEngine;

namespace ColorSelector
{
    public class ColorSelection
    {
        private float _h;
        private float _s;
        private float _v;

        public Color Color { get; private set; }
        public Color HueColor { get; private set; }
        public Color SaturationColor { get; private set; }
        public Color ValueColor { get; private set; }

        public enum SelectionType
        {
            None = 0,
            Hue,
            Value,
            Saturation,
            Color
        }

        public ColorSelection(float h, float s, float v)
        {
            _h = h;
            _s = s;
            _v = v;
            
            UpdateColor();
        }
        public void SetColor(SelectionType selectionType, float value)
        {
            var clamped = Mathf.Clamp01(value);

            switch (selectionType)
            {
                case SelectionType.Hue:
                {
                    _h = clamped;
                    break;
                }
                case SelectionType.Saturation:
                {
                    _s = clamped;
                    break;
                }
                case SelectionType.Value:
                {
                    _v = clamped;
                    break;
                }
                default:
                {
                    return;
                }
            }
            
            UpdateColor();
        }

        public void SetColor(Color color)
        {
            Color = color;
            Color.RGBToHSV(color, out _h, out _s, out _v);
            
            HueColor = Color.HSVToRGB(_h, 1f, 1f);
            SaturationColor = Color.HSVToRGB(_h, _s, 1f);
            ValueColor = Color.HSVToRGB(_h, 1f, _v);
        }

        public Color GetColor(SelectionType selectionType)
        {
            switch (selectionType)
            {
                case SelectionType.Hue: return HueColor;
                case SelectionType.Saturation: return SaturationColor;
                case SelectionType.Value: return ValueColor;
                default: return Color;
            }
        }

        public float GetSelectionValue(SelectionType selectionType)
        {
            switch (selectionType)
            {
                case SelectionType.Hue: return _h;
                case SelectionType.Saturation: return _s;
                case SelectionType.Value: return _v;
                default: return 0f;
            }
        }
        
        private void UpdateColor()
        {
            Color = Color.HSVToRGB(_h, _s, _v);
            HueColor = Color.HSVToRGB(_h, 1f, 1f);
            SaturationColor = Color.HSVToRGB(_h, _s, 1f);
            ValueColor = Color.HSVToRGB(_h, 1f, _v);
        }
    }
}
