using UnityEngine.EventSystems;

namespace HSVColorSelector
{
    public class SingleMaterialColorTarget : ColorTargetBase
    {
        protected override void HandleOnPointerClick(PointerEventData pointerEventData)
        {
            ColorController.SetColor(ApplyColor());
        }
    }
}
