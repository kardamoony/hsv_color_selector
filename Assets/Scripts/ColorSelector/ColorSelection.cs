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

        public ColorSelection SetColor(Color color)
        {
            Color = color;
            Color.RGBToHSV(color, out _h, out _s, out _v);
            
            HueColor = Color.HSVToRGB(_h, 1f, 1f);
            SaturationColor = Color.HSVToRGB(_h, _s, 1f);
            ValueColor = Color.HSVToRGB(_h, 1f, _v);

            return this;
        }

        public Color GetColor(SelectionType selectionType)
        {
            return selectionType switch
            {
                SelectionType.Hue => HueColor,
                SelectionType.Saturation => SaturationColor,
                SelectionType.Value => ValueColor,
                _ => Color
            };
        }

        public float GetSelectionValue(SelectionType selectionType)
        {
            return selectionType switch
            {
                SelectionType.Hue => _h,
                SelectionType.Saturation => _s,
                SelectionType.Value => _v,
                _ => 0f
            };
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
