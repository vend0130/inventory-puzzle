using System;
using Code.Extensions;
using Code.Utils.Hide;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.UI
{
    public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Button _button;
        [SerializeField] private Vector3 _enterScale = Vector3.one * 1.2f;
        [SerializeField] private float _duration = .08f;
        [SerializeField] private bool _notSwitched = true;

        [SerializeField, Hide(nameof(_notSwitched))]
        private Image _image;

        [SerializeField, Hide(nameof(_notSwitched))]
        private Sprite _on;

        [SerializeField, Hide(nameof(_notSwitched))]
        private Sprite _off;

        public event Action ClickHandler;

        private readonly Vector3 _defaultScale = Vector3.one;

        private Tween _tween;

        private void Start()
        {
            _button.Add(Click);
        }

        private void OnDestroy()
        {
            _tween.SimpleKill();
            _button.Add(Click);
        }

        public void OnPointerEnter(PointerEventData eventData) =>
            Play(_enterScale);

        public void OnPointerExit(PointerEventData eventData) =>
            Play(_defaultScale);

        public void ChangeState(bool value)
        {
            if (_notSwitched)
                return;

            _image.sprite = value ? _on : _off;
        }

        private void Click() =>
            ClickHandler?.Invoke();

        private void Play(Vector3 target)
        {
            _tween.SimpleKill();
            _tween = transform.DOScale(target, _duration).SetEase(Ease.Linear);
        }
    }
}