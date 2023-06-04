using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Game.ItemInfo
{
    [Serializable]
    public class MenuButtonData : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Localize _localizePrefix;
        [SerializeField] private Localize _localizePostfix;

        public event Action<int> DownHandler;

        private int _index;

        public void OnPointerDown(PointerEventData _) =>
            DownHandler?.Invoke(_index);

        public void ChangeText(LocalizedString prefix, LocalizedString postfix)
        {
            _localizePrefix.SetTerm(prefix.mTerm);
            _localizePostfix.SetTerm(postfix.mTerm);
            _localizePostfix.gameObject.SetActive(true);
        }

        public void ChangeText(LocalizedString prefix)
        {
            _localizePrefix.SetTerm(prefix.mTerm);
            _localizePostfix.gameObject.SetActive(false);
        }

        public void ChangeIndex(int index) =>
            _index = index;
    }
}