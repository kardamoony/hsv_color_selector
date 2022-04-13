using UnityEngine;

namespace ColorSelector
{
    public interface ICursorInfo
    {
        void UpdateCursor(in Transform cursorTransform);
    }
    
    public readonly struct CursorPositionInfo : ICursorInfo
    {
        private readonly Vector3 _worldPosition;

        public CursorPositionInfo(Vector3 worldPosition)
        {
            _worldPosition = worldPosition;
        }

        public void UpdateCursor(in Transform cursorTransform)
        {
            cursorTransform.position = _worldPosition;
        }
    }
    
    public readonly struct CursorRotationInfo : ICursorInfo
    {
        private readonly bool _isWorldRotation;
        private readonly Vector3 _rotationAxis;
        private readonly float _angle;

        public CursorRotationInfo(float angle, Vector3 axis, bool isWorldRotation = false)
        {
            _angle = angle;
            _rotationAxis = axis;
            _isWorldRotation = isWorldRotation;
        }
        
        public void UpdateCursor(in Transform cursorTransform)
        {
            var rotation = Quaternion.Euler(_rotationAxis * _angle);

            if (_isWorldRotation)
            {
                cursorTransform.rotation = rotation;
                return;
            }

            cursorTransform.localRotation = rotation;
        }
    }
}
