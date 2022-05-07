using UnityEngine;

namespace MaterialController
{
    public class MaterialPropertiesColorController : MonoBehaviour, IMaterialColorController
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private string _colorPropertyName = "_BaseColor";

        private int _colorPropertyId;
        private MaterialPropertyBlock _propertyBlock;

        public void SetColor(Color color)
        {
            _propertyBlock.SetColor(_colorPropertyId, color);
            _renderer.SetPropertyBlock(_propertyBlock);
        }

        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
            _colorPropertyId = Shader.PropertyToID(_colorPropertyName);
        }
        
#if UNITY_EDITOR
        private void Reset()
        {
            _renderer = GetComponent<Renderer>();
        }
#endif
    }
}
