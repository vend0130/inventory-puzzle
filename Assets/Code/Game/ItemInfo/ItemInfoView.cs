using Code.Data;
using UnityEngine;

namespace Code.Game.ItemInfo
{
    public class ItemInfoView : MonoBehaviour, IInfo
    {
        [SerializeField] private ItemInfoPanel _infoGun;
        [SerializeField] private ItemInfoPanel _infoMagazine;
        [SerializeField] private LockView _backgroundLock;

        private void Start()
        {
            _backgroundLock.CloseHandler += _infoGun.Close;
            _backgroundLock.CloseHandler += _infoMagazine.Close;
        }

        private void OnDestroy()
        {
            _backgroundLock.CloseHandler -= _infoGun.Close;
            _backgroundLock.CloseHandler -= _infoMagazine.Close;
        }

        public void Open(GunData data) =>
            _infoGun.Open(data.GetTexts());

        public void Open(MagazineData data) =>
            _infoMagazine.Open(data.GetTexts());
    }
}