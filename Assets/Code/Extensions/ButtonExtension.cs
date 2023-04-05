using UnityEngine.Events;
using UnityEngine.UI;

namespace Code.Extensions
{
    public static class ButtonExtension
    {
        public static void Add(this Button button, UnityAction call) =>
            button.onClick.AddListener(call);

        public static void Remove(this Button button, UnityAction call) =>
            button.onClick.RemoveListener(call);
        
        public static void RemoveAll(this Button button) =>
            button.onClick.RemoveAllListeners();

    }
}