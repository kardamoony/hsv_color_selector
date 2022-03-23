using UnityEngine;

namespace ColorPicker.Components
{
    public abstract class ColorSelectorComponent : MonoBehaviour
    {
        public virtual void OnColorChanged(ColorSelection colorSelection) { }
    }
}
