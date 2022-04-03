using UnityEngine;

namespace ColorSelector.Components
{
    public class ColorCursorRotating : ColorCursor
    {
        [SerializeField] private Vector3 _rotationAxis = Vector3.forward;
        
        public override void UpdatePosition(ColorCursorData data)
        {
            CursorTransform.localRotation = Quaternion.Euler(_rotationAxis * data.Angle);
        }
    }
}
