using ColorSelector.Interfaces;
using UnityEngine;

namespace ColorSelector.Components
{
    public abstract class ColorSelectorComponent : MonoBehaviour, IColorChangeListener
    {
        public virtual void OnColorChanged(ColorSelection colorSelection) { }
    }
}
