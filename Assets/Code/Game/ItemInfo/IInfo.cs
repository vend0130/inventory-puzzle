using Code.Data;

namespace Code.Game.ItemInfo
{
    public interface IInfo
    {
        void Open(GunData data);
        void Open(MagazineData data);
        void Open(BipodData data);
    }
}