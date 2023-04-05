using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.Services.LoadScene
{
    public interface ICurtain
    {
        UniTask FadeOn();
        UniTask FadeOff();
        void On();
    }
}