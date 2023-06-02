using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Plugins.Yandex
{
    public class YandexManager : MonoBehaviour
    {
        public static event Action<string> LoadedHandler;

        private static YandexManager _yandexObject;

        public void IsLoaded(string data) =>
            LoadedHandler?.Invoke(data);

        public static void CallSave(string data) =>
            Save(data);

        //call in .jslib
        public static void CallLoad()
        {
            if (_yandexObject == null)
            {
                _yandexObject = new GameObject(nameof(YandexManager)).AddComponent<YandexManager>();
                DontDestroyOnLoad(_yandexObject);
            }

            Load();
        }

        [DllImport("__Internal")]
        private static extern void Save(string data);

        [DllImport("__Internal")]
        private static extern void Load();
    }
}