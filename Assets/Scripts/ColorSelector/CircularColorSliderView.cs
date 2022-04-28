using UnityEngine;

namespace Selector
{
    public class CircularColorSliderView : CircularColorSelectorView
    {
        private static readonly int SectorProperty = Shader.PropertyToID("_Sector");
        private static readonly int RotateProperty = Shader.PropertyToID("_Rotate");

        private static readonly int FlipXProperty = Shader.PropertyToID("_FlipX");
        private static readonly int FlipYProperty = Shader.PropertyToID("_FlipY");

        [Range(0f, 360f)]
        [SerializeField] private float _startAngle = 10;
        [Range(0f, 360f)]
        [SerializeField] private float _endAngle = 170;

        [Range(0f, 10f)]
        [SerializeField] private float _clickAngleTolerance = 5f;

        [Space(10f)]
        [SerializeField] private bool _reverseValue;
        [SerializeField] private bool _flipX;
        [SerializeField] private bool _flipY;

        protected override void CreateControllers(ColorSelectionModel model)
        {
            var @params = new CircularColorSliderParams
            {
                RectTransform = RectTransform,
                RotationAxis = RotationAxis,
                RotationZeroAxis = ZeroRotation,
                    
                DistanceTolerance = ClickDistanceTolerance,
                AngleTolerance = _clickAngleTolerance,
                
                StartAngle = _startAngle,
                EndAngle = _endAngle,
                ReverseValue = _reverseValue,
                
                FlipX = _flipX
            };

            Controller = new CircularColorSliderController(model, ColorValueType, @params);
        }

        protected override void UpdateMaterialOnValidated(Material material)
        {
            base.UpdateMaterialOnValidated(material);
            material.SetFloat(RotateProperty, _startAngle);
            material.SetFloat(SectorProperty, _endAngle - _startAngle);
            material.SetFloat(FlipXProperty, _flipX ? 1f : 0f);
            material.SetFloat(FlipYProperty, _flipY ? 1f : 0f);
        }
    }
}
