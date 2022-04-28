using MVC;

namespace Selector
{
    public abstract class ColorSelectionController : ViewControllerBase<ColorSelectionModel>
    {
        protected ColorSelectionModel.ColorValueType ColorValueType { get; }

        protected ColorSelectionController(ColorSelectionModel model, ColorSelectionModel.ColorValueType colorValueType) : base(model)
        {
            ColorValueType = colorValueType;
        }
    }
}
