using System;
using Code.Data;
using Code.Data.Audio;
using Code.Infrastructure.Services.Audio;
using Code.Infrastructure.Services.Progress;
using Code.UI.SelectLevel;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class SimplyMenu : MonoBehaviour
    {
        [SerializeField] private Localize _levelLocalize;
        [SerializeField] private TMP_Text _valueLevelText;
        [SerializeField] private SelectLevelUI _selectLevelUI;
        [field: SerializeField] public MenuButton AgainButton { get; private set; }
        [field: SerializeField] public MenuButton SoundButton { get; private set; }

        [Header("Languages")] [SerializeField] private Button _languageButton;
        [SerializeField] private Image _languageButtonImage;
        [SerializeField] private Sprite _russianSprite;
        [SerializeField] private Sprite _englishSprite;

        public event Action<int> PlayLevelHandler;

        private const string LevelTextPostfix = "{0} <size=25><color=#7E7E7E>/ {1}</color></size>";

        private IAudioService _audioService;

        private void Awake()
        {
            _selectLevelUI.ClickHandler += OnPlayLevel;
            _languageButton.onClick.AddListener(ChangeLanguage);
        }

        private void OnDestroy()
        {
            _selectLevelUI.ClickHandler -= OnPlayLevel;
            _languageButton.onClick.RemoveListener(ChangeLanguage);
        }

        public void Init(
            int currentLevel,
            int maxLevel,
            LocalizedString level,
            IAudioService audioService,
            LevelsData levelsData,
            IProgressService progressService)
        {
            _audioService = audioService;

            _levelLocalize.SetTerm(level.mTerm);
            _valueLevelText.text = string.Format(LevelTextPostfix, currentLevel, maxLevel);

            ChangeSprite();

            _selectLevelUI.Init(levelsData, progressService);
        }

        private void ChangeLanguage()
        {
            _audioService.Play(SoundType.Button);
            LocalizationManager.CurrentLanguageCode = LocalizationManager.CurrentLanguageCode == "en" ? "ru" : "en";
            ChangeSprite();
        }

        private void OnPlayLevel(int level) =>
            PlayLevelHandler?.Invoke(level);

        private void ChangeSprite()
        {
            _languageButtonImage.sprite = LocalizationManager.CurrentLanguageCode == "en"
                ? _englishSprite
                : _russianSprite;
        }
    }
}