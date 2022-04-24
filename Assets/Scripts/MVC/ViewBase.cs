namespace MVC
{
    public abstract class ViewBase<TModel> : UnityEngine.MonoBehaviour where TModel : ModelBase
    {
        private TModel _model;

        private bool _initialized;

        public void Initialize(TModel model)
        {
            if (_initialized || model == null) return;
            _model = model;
            
            CreateControllers(model);
            Subscribe(model);
            
            _initialized = true;
            OnInitialized(model);
        }

        protected abstract void CreateControllers(TModel model);
        protected abstract void SubscribeToModel(TModel model);
        protected abstract void UnSubscribeFromModel(TModel model);

        /// <summary>
        /// base method is empty, so there's no need to call base.OnSubscribed()
        /// </summary>
        protected virtual void OnSubscribed(){}
        
        /// <summary>
        /// base method is empty, so there's no need to call base.OnUnSubscribed()
        /// </summary>
        protected virtual void OnUnSubscribed(){}

        /// <summary>
        /// base method is empty, so there's no need to call base.OnInitialized()
        /// </summary>
        protected virtual void OnInitialized(TModel model){}
        
        /// <summary>
        /// base method is empty, so there's no need to call base.OnDeInitialized()
        /// </summary>
        protected virtual void OnDeInitialized(){}
        
        private void DeInitialize()
        {
            if (!_initialized && _model == null)
            {
                OnDeInitialized();
                return;
            }
            
            Unsubscribe(_model);
            _model = null;
            _initialized = false;
            OnDeInitialized();
        }

        private void Subscribe(TModel model)
        {
            SubscribeToModel(model);
            OnSubscribed();
        }

        private void Unsubscribe(TModel model)
        {
            UnSubscribeFromModel(model);
            OnUnSubscribed();
        }

        private void OnDestroy()
        {
            DeInitialize();
        }
    }
}
