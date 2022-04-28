using UnityEngine;

namespace Selector
{
    public abstract class ColorPreviewBase : MonoBehaviour
    {
        [SerializeField] protected ColorSelectionModel.ColorValueType ColorValueType;

        public abstract void OnColorChanged(ColorSelectionModel model);
    }
}
