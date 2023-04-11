using System;
using System.Collections.Generic;
using Code.Game.Cells;
using Code.Game.Item;
using Code.Game.Item.Items;
using UnityEngine;

namespace Code.Game.ItemInfo
{
    public class ItemMenu : MonoBehaviour
    {
        [field: SerializeField] public ItemInfoView Info { get; private set; }

        [SerializeField] private MenuButtonData[] _buttonsData;
        [SerializeField] private GameObject _menu;
        [SerializeField] private LockView _backgroundLock;

        public event Action OpenInfoHandler;
        public event Action<BaseItem, int> CreateItemHandler;

        private const string InformationText = "ИНФОРМАЦИЯ";
        private const string PrefixText = "Снять";

        private BaseItem _lastItem;

        private void Start()
        {
            _menu.SetActive(false);
            _buttonsData[0].Text.text = InformationText;

            for (int i = 0; i < _buttonsData.Length; i++)
            {
                _buttonsData[i].ChangeIndex(i);

                if (i == 0)
                    _buttonsData[i].DownHandler += OpenInformation;
                else
                    _buttonsData[i].DownHandler += Buttons;
            }

            _backgroundLock.CloseHandler += Close;
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _buttonsData.Length; i++)
            {
                if (i == 0)
                    _buttonsData[i].DownHandler -= OpenInformation;
                else
                    _buttonsData[i].DownHandler -= Buttons;
            }

            _backgroundLock.CloseHandler -= Close;
        }

        public void Open(Vector2 position, BaseItem item)
        {
            _lastItem = item;
            _backgroundLock.On();

            _buttonsData[0].gameObject.SetActive(true);

            if (item.AdditionalDatas != null)
            {
                for (int i = 0; i < item.AdditionalDatas.Length; i++)
                {
                    if (!item.AdditionalDatas[i].Activate)
                        continue;

                    _buttonsData[i + 1].Text.text = $"{PrefixText} {GetText(item.AdditionalDatas[i].AdditionalType)}";
                    _buttonsData[i + 1].gameObject.SetActive(true);
                }
            }

            _menu.transform.position = position;
            _menu.SetActive(true);
        }

        private void OpenInformation(int _)
        {
            Close();
            OpenInfoHandler?.Invoke();
        }

        private void Buttons(int index)
        {
            _lastItem.ChangeAdditionalState(index - 1, false);
            CreateItemHandler?.Invoke(_lastItem, (index - 1));

            _lastItem.ParentCells.ForEach((cell) => cell.RemoveItem());
            if (CellsHelper.TryEnterOnCell(_lastItem.CurrentInventor, _lastItem, out List<CellView> cells))
                _lastItem.ChangeCell(cells);
            _lastItem.ParentCells.ForEach((cell) => cell.AddItem(_lastItem));

            _lastItem = null;
            _backgroundLock.Close();
        }

        private void Close()
        {
            foreach (MenuButtonData buttonData in _buttonsData)
                buttonData.gameObject.SetActive(false);

            _menu.SetActive(false);
            _lastItem = null;
        }

        private string GetText(AdditionalType type)
        {
            switch (type)
            {
                case AdditionalType.Magazine:
                    return "Магазин"; //note: move to constants file
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}