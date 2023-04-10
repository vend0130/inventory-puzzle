using UnityEditor;
using UnityEngine;

namespace Code.Infrastructure.StateMachine.States
{
    public class ExitState : IDefaultState
    {
        public void Enter()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void Exit()
        {
        }
    }
}