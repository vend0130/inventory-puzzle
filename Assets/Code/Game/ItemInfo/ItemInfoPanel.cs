using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Game.ItemInfo
{
    public class ItemInfoPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [Space, SerializeField] private List<TextPair> _texts;

        private void Start() =>
            _panel.SetActive(false);

        public void Open(List<(string, string)> datas)
        {
            if (datas.Count != _texts.Count)
                throw new Exception("not correct data");

            for (var i = 0; i < _texts.Count; i++)
            {
                if (_texts[i].First != null)
                    _texts[i].First.text = datas[i].Item1;
                _texts[i].Second.text = datas[i].Item2;
            }

            _panel.SetActive(true);
        }

        public void Close() =>
            _panel.SetActive(false);
    }
}