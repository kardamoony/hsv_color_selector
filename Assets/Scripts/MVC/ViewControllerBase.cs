namespace MVC
{
    public abstract class ViewControllerBase<TModel> where TModel : ModelBase
    {
        protected TModel Model { get; }
        
        protected ViewControllerBase(TModel model)
        {
            Model = model;
        }
    }
}
