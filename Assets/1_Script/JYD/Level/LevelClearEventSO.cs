using System;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "LevelClearEventSO", menuName = "SO/LevelClearEvent")]
    public class LevelClearEventSO : ScriptableObject
    {
        public Action LevelClearEvent;
        public Action<string,Action> SceneMoveEvent;

    }
}
