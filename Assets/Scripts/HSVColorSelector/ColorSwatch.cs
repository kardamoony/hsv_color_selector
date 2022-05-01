using System;
using UnityEngine;
using UnityEngine.UI;

namespace HSVColorSelector
{
    public class ColorSwatch : MonoBehaviour
    {
        public event Action<Color> OnClicked;
        
        [SerializeField] private ColorPreviewBase _colorPreview;
        [SerializeField] private Button _button;

        private GameObject _cachedGo;
        private Color _color;
        private bool _initialized;

        public void Activate(Color color)
        {
            _color = color;
            _colorPreview.SetColor(color);
            _initialized = true;
            _cachedGo.SetActive(true);
        }

        public void Deactivate()
        {
            _initialized = false;
            _cachedGo.SetActive(false);
        }
        
        private void HandleOnClick()
        {
            if (!_initialized) return;
            OnClicked?.Invoke(_color);
        }

        private void Awake()
        {
            _cachedGo = gameObject;
            _button.onClick.AddListener(HandleOnClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}
