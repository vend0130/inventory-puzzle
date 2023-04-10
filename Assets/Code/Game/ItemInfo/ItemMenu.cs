using System;
using Code.Extensions;
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
        
        private const string InformationText = "ИНФОРМАЦИЯ";

        private void Start()
        {
            _menu.SetActive(false);

            _buttonsData[0].Button.Add(OpenInformation);
            _buttonsData[0].Text.text = InformationText;

            _backgroundLock.CloseHandler += Close;
        }

        private void OnDestroy()
        {
            foreach (MenuButtonData buttonData in _buttonsData)
                buttonData.Button.RemoveAll();

            _backgroundLock.CloseHandler -= Close;
        }

        public void Open(Vector2 position)
        {
            _backgroundLock.On();

            _buttonsData[0].gameObject.SetActive(true);

            _menu.transform.position = position;
            _menu.SetActive(true);
        }

        private void OpenInformation()
        {
            Close();
            OpenInfoHandler?.Invoke();
        }

        private void Close()
        {
            foreach (MenuButtonData buttonData in _buttonsData)
                buttonData.gameObject.SetActive(false);

            _menu.SetActive(false);
        }
    }
}