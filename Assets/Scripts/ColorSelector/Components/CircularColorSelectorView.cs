using UnityEngine;

namespace ColorSelector.Components
{
    public abstract class CircularColorSelectorView : ColorSelectorView
    {
        private static readonly int OuterCircle = Shader.PropertyToID("_OuterCircle");
        private static readonly int InnerCircle = Shader.PropertyToID("_InnerCircle");
        
        [Range(0, 1)]
        [SerializeField] private float _innerCircle;
        [Range(0, 1)] 
        [SerializeField] private float _outerCircle;

        protected (float distance, float innerRadius, float outerRadius) GetCursorDistance(Vector3 center)
        {
            var maxRadius = GetMaxRadius(center);

            var outerRadius = maxRadius * _outerCircle;
            var innerRadius = maxRadius * _innerCircle;

            var distance = innerRadius + (outerRadius - innerRadius) * 0.5f;

            return (distance, innerRadius, outerRadius);
        }

        protected override void OnMaterialSetup(Material material)
        {
            material.SetFloat(OuterCircle, _outerCircle);
            material.SetFloat(InnerCircle, _innerCircle);
        }
    }
}
