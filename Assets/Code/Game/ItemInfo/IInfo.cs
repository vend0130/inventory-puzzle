using Code.Data;

namespace Code.Game.ItemInfo
{
    public interface IInfo
    {
        void Open();
        void Open(GunData data);
        void Close();
    }
}