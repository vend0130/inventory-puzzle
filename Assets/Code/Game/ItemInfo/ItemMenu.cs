using System;
using System.Collections.Generic;
using Code.Data.Audio;
using Code.Data.Localize;
using Code.Extensions;
using Code.Game.Cells;
using Code.Game.Item;
using Code.Game.Item.Items;
using Code.Infrastructure.Services.Audio;
using I2.Loc;
using UnityEngine;

namespace Code.Game.ItemInfo
{
    public class ItemMenu : MonoBehaviour
    {
        [field: SerializeField] public ItemInfoView Info { get; private set; }
        [field: SerializeField] public LockView LockView { get; private set; }
        [SerializeField] private MenuButtonData[] _buttonsData;
        [SerializeField] private GameObject _menu;
        [SerializeField] private LockView _backgroundLock;

        public event Action<BaseItem, int> CreateItemHandler;

        private IAudioService _audioService;
        private ItemMenuLocalizeConfig _localization;
        private BaseItem _lastItem;

        private void Start()
        {
            _menu.SetActive(false);
            _buttonsData[0].ChangeText(_localization.Information);

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

        public void Init(IAudioService audioService, ItemMenuLocalizeConfig localization)
        {
            _audioService = audioService;
            _localization = localization;
        }

        public void Open(Vector2 position, BaseItem item)
        {
            _audioService.Play(SoundType.Button);

            _lastItem = item;
            _backgroundLock.On();

            _buttonsData[0].gameObject.SetActive(true);

            if (item.AdditionalDatas != null)
            {
                for (int i = 0; i < item.AdditionalDatas.Length; i++)
                {
                    if (!item.AdditionalDatas[i].Activate)
                        continue;

                    _buttonsData[i + 1].ChangeText(_localization.Take,
                        GetText(item.AdditionalDatas[i].AdditionalType));
                    _buttonsData[i + 1].gameObject.SetActive(true);
                }
            }

            _menu.transform.position = position;
            _menu.SetActive(true);
        }

        private void OpenInformation(int _)
        {
            _audioService.Play(SoundType.Button);
            _lastItem.OpenInfo();
            Close();
        }

        private void Buttons(int index)
        {
            _audioService.Play(SoundType.Button);

            _lastItem.ParentCells.RemoveItemInCell();
            _lastItem.ParentCells.CellsExit();

            _lastItem.ChangeAdditionalState(index - 1, false);
            CreateItemHandler?.Invoke(_lastItem, (index - 1));

            if (CellsHelper.TryEnterOnCell(_lastItem.CurrentInventor, _lastItem, out List<ItemCellData> cells))
                _lastItem.ChangeCell(cells);

            _lastItem.ParentCells.AddItemInCell(_lastItem);
            _lastItem.ParentCells.CellsDrop();

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

        private LocalizedString GetText(AdditionalType type)
        {
            switch (type)
            {
                case AdditionalType.Magazine:
                    return _localization.Magazine;
                    ;
                case AdditionalType.Bipod:
                    return _localization.Bipods;
                case AdditionalType.AIM:
                    return _localization.Scope;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}