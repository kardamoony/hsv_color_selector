using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class UIPsressScaleTweener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Transform _scalableContent;
        [SerializeField] private float _pressedScale = 0.9f;
        [SerializeField] private float _scaleDuration = 0.05f;
        [SerializeField] private Ease _ease = Ease.Linear;

        private Tweener _tweener;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _tweener?.Kill();
            _tweener = GetScaleTweener(_pressedScale);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _tweener?.Kill();
            _tweener = GetScaleTweener(1f);
        }
        
        private Tweener GetScaleTweener(float scale) => _scalableContent.DOScale(scale, _scaleDuration).SetEase(_ease);

        private void OnDestroy()
        {
            _tweener?.Kill();
        }
    }
}
