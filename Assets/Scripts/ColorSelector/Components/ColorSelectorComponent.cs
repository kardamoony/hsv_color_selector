using UnityEngine;

namespace ColorSelector.Components
{
    public abstract class ColorSelectorComponent : MonoBehaviour
    {
        public abstract void OnColorChanged(ColorSelection colorSelection);
    }
}
