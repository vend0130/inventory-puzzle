using Plugins.Yandex;
using UnityEngine;

namespace Code.Infrastructure.Services.Ad
{
    public class AdYandexService : IAdService
    {
        private const float Cooldown = 123;

        private float? _nextTimeShow = null;

        public void Show()
        {
            if (_nextTimeShow == null)
            {
                ChangeNextTime();
                return;
            }
            
            if (Time.time <= _nextTimeShow)
                return;

            ChangeNextTime();
            YandexManager.ShowFullscreenAdv();
        }

        private void ChangeNextTime() => 
            _nextTimeShow = Time.time + Cooldown;
    }
}