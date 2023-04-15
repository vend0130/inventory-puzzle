using Code.Data;
using UnityEngine;

namespace Code.Game.ItemInfo
{
    public class ItemInfoView : MonoBehaviour, IInfo
    {
        [SerializeField] private ItemInfoPanel _infoGun;
        [SerializeField] private ItemInfoPanel _infoMagazine;
        [SerializeField] private ItemInfoPanel _infoBipod;
        [SerializeField] private LockView _backgroundLock;

        private void Start()
        {
            _backgroundLock.CloseHandler += _infoGun.Close;
            _backgroundLock.CloseHandler += _infoMagazine.Close;
            _backgroundLock.CloseHandler += _infoBipod.Close;
        }

        private void OnDestroy()
        {
            _backgroundLock.CloseHandler -= _infoGun.Close;
            _backgroundLock.CloseHandler -= _infoMagazine.Close;
            _backgroundLock.CloseHandler -= _infoBipod.Close;
        }

        public void Open(GunData data) =>
            _infoGun.Open(data.GetTexts());

        public void Open(MagazineData data) =>
            _infoMagazine.Open(data.GetTexts());

        public void Open(BipodData data) =>
            _infoBipod.Open(data.GetTexts());
    }
}