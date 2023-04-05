using DG.Tweening;

namespace Code.Extensions
{
    public static class DoTweenExtension
    {
        public static void SimpleKill(this Tween tween)
        {
            if (tween == null || !tween.active)
                return;

            tween.Kill();
        }
    }
}