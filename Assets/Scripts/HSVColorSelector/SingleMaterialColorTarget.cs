using UnityEngine;

namespace HSVColorSelector
{
    public class SingleMaterialColorTarget : ColorTargetBase
    {
        public override void ApplyColor(Color color)
        {
            ColorController.SetColor(color);
        }
    }
}
