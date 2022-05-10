using UnityEngine;

namespace MaterialController
{
    public class MaterialPropertiesColorController : MonoBehaviour, IMaterialColorController
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private string _colorPropertyName = "_BaseColor";

        private int _colorPropertyId;
        private MaterialPropertyBlock _propertyBlock;

        private Color _currentColor;

        public void SetColor(Color color)
        {
            _currentColor = color;
            
            _propertyBlock.SetColor(_colorPropertyId, color);
            _renderer.SetPropertyBlock(_propertyBlock);
        }

        public Color GetColor()
        {
            return _currentColor;
        }

        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
            _colorPropertyId = Shader.PropertyToID(_colorPropertyName);
            _currentColor = _renderer.sharedMaterial.GetColor(_colorPropertyId);
        }
        
#if UNITY_EDITOR
        private void Reset()
        {
            _renderer = GetComponent<Renderer>();
        }
#endif
    }
}
