using System;
using System.Collections.Generic;
using Code.Data.Audio;
using Code.Extensions;
using Code.Infrastructure.Services.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Game.ItemInfo
{
    public class LockView : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private GameObject _current;
        [SerializeField] private List<Button> _closeButtons;
        private IAudioService _audioService;

        public event Action CloseHandler;

        private void Awake()
        {
            foreach (Button button in _closeButtons)
                button.Add(Close);

            _current.SetActive(false);
        }

        private void OnDestroy()
        {
            foreach (Button button in _closeButtons)
                button.Remove(Close);
        }

        public void OnPointerDown(PointerEventData _) =>
            Close();

        public void InitAudioService(IAudioService audioService) =>
            _audioService = audioService;

        public void On() =>
            _current.SetActive(true);

        public void Close()
        {
            _audioService.Play(SoundType.Button);

            CloseHandler?.Invoke();
            _current.SetActive(false);
        }
    }
}