using System;
using System.Collections.Generic;
using I2.Loc;
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
                    _texts[i].First.SetTerm(datas[i].Item1.mTerm);

                _texts[i].Second.SetTerm(datas[i].Item2.mTerm);
            }

            _panel.SetActive(true);
        }

        public void Close() =>
            _panel.SetActive(false);
    }
}