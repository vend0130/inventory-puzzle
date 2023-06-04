using System;
using System.Collections.Generic;
using I2.Loc;
using TMPro;
using UnityEngine;

namespace Code.Game.ItemInfo
{
    public class ItemInfoPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [Space, SerializeField] private List<LocalizePair> _texts;

        private void Start() =>
            _panel.SetActive(false);

        public void Open(List<(LocalizedString, LocalizedString)> datas)
        {
            if (datas.Count != _texts.Count)
                throw new Exception("not correct data");

            for (var i = 0; i < _texts.Count; i++)
            {
                if (_texts[i].First != null)
                    SetText(_texts[i].First, _texts[i].FirstTMP, datas[i].Item1);

                SetText(_texts[i].Second, _texts[i].SecondTMP, datas[i].Item2);
            }

            _panel.SetActive(true);
        }

        public void Close() =>
            _panel.SetActive(false);

        private void SetText(Localize localize, TMP_Text tmpText, LocalizedString localizedString)
        {
            if (localizedString.mTerm.StartsWith("item"))
                localize.SetTerm(localizedString.mTerm);
            else
                tmpText.text = localizedString.mTerm;
        }
    }
}