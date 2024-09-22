using System;
using System.Collections.Generic;
using Code.Data;
using Code.Infrastructure.Services.Progress;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.SelectLevel
{
    public class SelectLevelUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _container;
        [SerializeField] private Button _openButton;
        [SerializeField] private Button _closeButton;

        [SerializeField] private SelectLevelButton _prefab;
        [SerializeField] private Transform _parent;

        public event Action<int> ClickHandler;

        private readonly List<SelectLevelButton> _buttons = new();

        private LevelsData _levelsData;
        private IProgressService _progressService;

        public void Init(LevelsData levelsData, IProgressService progressService)
        {
            _levelsData = levelsData;
            _progressService = progressService;
        }

        private void Start()
        {
            _openButton.onClick.AddListener(OnOpen);
            _closeButton.onClick.AddListener(OnClose);

            int openedLevel = _progressService.ProgressData.OpenedLevel;
            int currentLevel = _progressService.ProgressData.CurrentLevel;

            for (int i = 0; i < _levelsData.CountLevels; i++)
            {
                SelectLevelButton level = Instantiate(_prefab, _parent);
                level.Set(i, i > openedLevel, i == currentLevel);
                _buttons.Add(level);

                level.ClickHandler += OnClick;
            }

            OnClose();
        }

        private void OnDestroy()
        {
            _openButton.onClick.RemoveListener(OnOpen);
            _closeButton.onClick.RemoveListener(OnClose);

            for (int i = 0; i < _buttons.Count; i++)
                _buttons[i].ClickHandler -= OnClick;
        }

        private void OnOpen()
        {
            _container.alpha = 1;
            _container.gameObject.SetActive(true);
        }

        private void OnClose()
        {
            _container.alpha = 0;
            _container.gameObject.SetActive(false);
        }

        private void OnClick(int level) =>
            ClickHandler?.Invoke(level);
    }
}