using TMPro;
using UnityEngine;

namespace Code.Utils
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _tmp;

        private const float FpsMeasurePeriod = 0.5f;

        private readonly string _display = "{0} FPS";

        private int _fpsAccumulator;
        private float _fpsNextPeriod;
        private int _currentFps;
        
        private void Start()
        {
            _fpsNextPeriod = Time.realtimeSinceStartup + FpsMeasurePeriod;
        }

        private void Update()
        {
            _fpsAccumulator++;

            if (Time.realtimeSinceStartup <= _fpsNextPeriod)
                return;

            _currentFps = (int)(_fpsAccumulator / FpsMeasurePeriod);
            _fpsAccumulator = 0;
            _fpsNextPeriod += FpsMeasurePeriod;
            _tmp.text = string.Format(_display, _currentFps);
        }
    }
}