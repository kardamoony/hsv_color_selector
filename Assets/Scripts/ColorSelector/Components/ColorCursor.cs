using UnityEngine;

namespace ColorSelector.Components
{
    public class ColorCursor : MonoBehaviour
    {
        [SerializeField] protected Transform CursorTransform;

        public virtual void UpdatePosition(ColorCursorData data)
        {
            CursorTransform.position = data.Position;
        }
    }
}
