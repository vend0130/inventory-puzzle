using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.SelectLevel
{
    public class SelectLevelButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Image _currentLevel;

        public event Action<int> ClickHandler;
        public int Level { get; private set; }

        private void Start() =>
            _button.onClick.AddListener(OnClick);

        private void OnDestroy() =>
            _button.onClick.RemoveListener(OnClick);

        public void Set(int level, bool isLocked, bool isCurrent)
        {
            Level = level;

            _levelText.gameObject.SetActive(!isLocked);
            _iconImage.gameObject.SetActive(isLocked);
            _currentLevel.gameObject.SetActive(isCurrent);
            _button.interactable = !isLocked;

            _levelText.text = (Level + 1).ToString();
        }

        private void OnClick() =>
            ClickHandler?.Invoke(Level);
    }
}