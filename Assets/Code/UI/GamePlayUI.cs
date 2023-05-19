using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class GamePlayUI : MonoBehaviour
    {
        [field: SerializeField] public Button ExitButton { get; private set; }

        private void Awake()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            gameObject.SetActive(false);
#endif
        }
    }
}