using I2.Loc;
using TMPro;
using UnityEngine;

namespace Code.UI
{
    public class SimplyMenu : MonoBehaviour
    {
        [SerializeField] private Localize _levelLocalize;
        [SerializeField] private TMP_Text _valueLevelText;
        [field: SerializeField] public MenuButton AgainButton { get; private set; }
        [field: SerializeField] public MenuButton SoundButton { get; private set; }
        [field: SerializeField] public MenuButton LanguageButton { get; private set; }

        private const string LevelTextPostfix = "{0} <size=25><color=#7E7E7E>/ {1}</color></size>";

        public void Init(int currentLevel, int maxLevel, LocalizedString level)
        {
            _levelLocalize.SetTerm(level.mTerm);
            _valueLevelText.text = string.Format(LevelTextPostfix, currentLevel, maxLevel);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                LocalizationManager.CurrentLanguageCode = "en";
            else if (Input.GetKeyDown(KeyCode.W))
                LocalizationManager.CurrentLanguageCode = "ru";
        }
    }
}