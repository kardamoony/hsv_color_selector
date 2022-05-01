using System;
using UnityEngine;

namespace MaterialController
{
    public class SharedMaterialColorController : MonoBehaviour, IMaterialColorController
    {
        [SerializeField] private string _colorPropertyName = "_BaseColor";
        [SerializeField] private Material _material;

        private int _colorPropertyId;
        
        public void SetColor(Color color)
        {
            _material.SetColor(_colorPropertyId, color);
        }

        private void Awake()
        {
            _colorPropertyId = Shader.PropertyToID(_colorPropertyName);
        }
    }
}
