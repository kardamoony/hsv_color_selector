using UnityEngine;

namespace ColorSelector.Components
{
    public abstract class ColorSelectorComponentBase : MonoBehaviour
    {
        public abstract void UpdateViewOnColorChanged(ColorSelection colorSelection);
    }
}
