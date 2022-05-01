using UnityEngine;

namespace HSVColorSelector
{
    public abstract class ColorPreviewBase : MonoBehaviour
    {
        [SerializeField] protected ColorValueType ColorValueType;
        
        public abstract void OnColorChanged(ColorSelectionModel model);
        public abstract void SetColor(Color color);
    }
}