using Code.Data;
using Code.Extensions;
using Plugins.Yandex;
using UnityEngine;

namespace Code.Infrastructure.Services.SaveLoad
{
    public class DataResetHelper : MonoBehaviour
    {
        private bool _isDown;
        private float _downTime;

        private static DataResetHelper _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                return;
            }

            Destroy(gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _isDown = true;
                _downTime = Time.time;
            }
            else if (Input.GetKeyUp(KeyCode.Q) && _isDown)
            {
                _isDown = false;

                if (Time.time - _downTime > 3)
                    Save();
            }

            if (_isDown && Time.time - _downTime > 3)
            {
                _isDown = false;
                Save();
            }
        }

        private void Save()
        {
            ProgressData data = new ProgressData();
            YandexManager.CallSave(data.ToJson());
        }
    }
}