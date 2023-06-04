using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class GamePlayUI : MonoBehaviour
    {
        [field: SerializeField] public Button ExitButton { get; private set; }
        [SerializeField] private Localize _localize;

        private void Awake()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            gameObject.SetActive(false);
#endif
        }

        public void ChangeText(LocalizedString localizedString) =>
            _localize.SetTerm(localizedString.mTerm);
    }
}