using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonPressScaleTween : MonoBehaviour
    {
        [SerializeField] private Button _button;
        //[SerializeField]

        private Tweener _tweener;

        private void HandleOnButtonClicked()
        {
            //_button.transform
        }

        private void Awake()
        {
           // _button.on
        }

        private void Reset()
        {
            _button = GetComponent<Button>();
        }
    }
}
