using UnityEngine;

namespace ColorSelector
{
    public abstract class ColorPreviewBase : MonoBehaviour
    {
        [SerializeField] protected ColorSelectionModel.ColorValueType ColorValueType;

        public abstract void OnColorChanged(ColorSelectionModel model);
    }
}
