using System.Globalization;
using Code.Data;
using TMPro;
using UnityEngine;

namespace Code.Game.ItemInfo
{
    public class ItemInfoView : MonoBehaviour, IInfo
    {
        [SerializeField] private GameObject _info;
        [SerializeField] private LockView _backgroundLock;

        [Space, SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private TextMeshProUGUI _calibre;
        [SerializeField] private TextMeshProUGUI _size;
        [SerializeField] private TextMeshProUGUI _sightingRange;
        [SerializeField] private TextMeshProUGUI _maxDistance;
        [SerializeField] private TextMeshProUGUI _typeArmo;

        private GameObject _currentPanel;

        private void Start()
        {
            _info.SetActive(false);

            _backgroundLock.CloseHandler += Close;
        }

        private void OnDestroy() =>
            _backgroundLock.CloseHandler -= Close;

        public void Open()
        {
            //TODO: текст - неизвестный объект
        }

        public void Open(GunData data)
        {
            _name.text = data.Name;
            _description.text = data.Description;
            _calibre.text = data.Calibre.ToString(CultureInfo.InvariantCulture);
            _size.text = data.Size.ToString(CultureInfo.InvariantCulture);
            _sightingRange.text = data.SightingRange.ToString(CultureInfo.InvariantCulture);
            _maxDistance.text = data.MaxDistance.ToString(CultureInfo.InvariantCulture);
            _typeArmo.text = data.TypeArmo;

            _info.SetActive(true);

            _currentPanel = _info;
        }

        public void Close() =>
            _currentPanel.SetActive(false);
    }
}