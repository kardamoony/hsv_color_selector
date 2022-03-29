using UnityEngine;

namespace ColorSelector.Components
{
    public abstract class ColorSelectorComponent : MonoBehaviour
    {
        public virtual void OnColorChanged(ColorSelection colorSelection) { }
    }
}
