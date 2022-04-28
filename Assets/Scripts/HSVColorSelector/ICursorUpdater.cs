using UnityEngine;

namespace HSVColorSelector
{
    public interface ICursorUpdater
    {
        void UpdateCursor(in Transform cursor);
    }

    public struct CursorNullUpdater : ICursorUpdater
    {
        public void UpdateCursor(in Transform cursor){ }
    }
    
    public struct CursorPositionUpdater : ICursorUpdater
    {
        private Vector2 _position;

        public CursorPositionUpdater(Vector2 position)
        {
            _position = position;
        }

        public void UpdateCursor(in Transform cursor)
        {
            cursor.position = _position;
        }
    }

    public struct CursorRotationUpdater : ICursorUpdater
    {
        private readonly bool _isWorldRotation;
        private readonly Vector3 _rotationAxis;
        private readonly float _angle;

        public CursorRotationUpdater(float angle, Vector3 axis, bool isWorldRotation = false)
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