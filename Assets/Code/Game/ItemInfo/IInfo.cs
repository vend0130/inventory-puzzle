using Code.Data;
using Code.Data.Items;

namespace Code.Game.ItemInfo
{
    public interface IInfo
    {
        void Open(GunData data);
        void Open(MagazineData data);
        void Open(BipodData data);
        void Open(SimpleData data);
        void Open(AimData data);
    }
}