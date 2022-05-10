using UnityEngine;

namespace MaterialController
{
    public class SharedMaterialColorController : MonoBehaviour, IMaterialColorController
    {
        [SerializeField] private string _colorPropertyName = "_BaseColor";
        [SerializeField] private Material _material;

        private int _colorPropertyId;
        private Color _initialColor;
        
        public void SetColor(Color color)
        {
            _material.SetColor(_colorPropertyId, color);
        }

        public Color GetColor()
        {
            return _material.GetColor(_colorPropertyId);
        }

        private void Awake()
        {
            _colorPropertyId = Shader.PropertyToID(_colorPropertyName);
            _initialColor = _material.GetColor(_colorPropertyId);
        }

        private void OnDestroy()
        {
            SetColor(_initialColor);
        }
        
#if UNITY_EDITOR
        private void Reset()
        {
            var r = GetComponent<Renderer>();
            if (r) _material = r.sharedMaterial;
        }
#endif
    }
}
