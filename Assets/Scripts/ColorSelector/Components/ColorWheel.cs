using ColorPicker.Components;
using ColorPicker.StaticHelpers;
using UnityEngine;

namespace ColorPicker
{
    [ExecuteInEditMode]
    public class ColorWheel : ColorValueController
    {
        private static readonly int BackgroundCircle = Shader.PropertyToID("_BackgroundCircle");

        [Range(0, 1)] 
        [SerializeField] private float _backgroundSize = 1f;
        [Range(0, 1)] 
        [SerializeField] private float _wheelOuterSize = 0.9f;
        [Range(0, 1)] 
        [SerializeField] private float _wheelInnerSize = 0.7f;

        
        protected override bool IsClickValid(Vector3? position, Vector3 center, out Vector3 cursorPosition, out float colorValue)
        {
            cursorPosition = Vector3.zero;
            colorValue = 1f;
            
            if (!position.HasValue) return false;

            var pos = position.Value;
            
            var maxRadius = GetMaxRadius(center);

            var outerRadius = maxRadius * _wheelOuterSize;
            var innerRadius = maxRadius * _wheelInnerSize;
            
            var cursorDistance = innerRadius + (outerRadius - innerRadius) * 0.5f;
            var cursorDirection = (pos - center).normalized;
            cursorPosition = center + cursorDirection * cursorDistance;
            
            var clickDistance = Vector3.Distance(center, pos);

            var isValid = clickDistance >= innerRadius && clickDistance <= outerRadius;
            
            var angle = MathHelper.GetAngle360(pos, center, Vector2.up);
            colorValue = ColorHelper.Angle360ToHue(angle);

            return isValid;
        }

        protected override void OnMaterialSetup(Material material)
        {
            material.SetFloat(OuterCircle, _wheelOuterSize);
            material.SetFloat(InnerCircle, _wheelInnerSize);
            material.SetFloat(BackgroundCircle, _backgroundSize);
        }
    }
}
