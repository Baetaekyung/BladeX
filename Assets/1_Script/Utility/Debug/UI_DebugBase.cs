using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BossRushGame
{
    public abstract class UI_DebugBase<T> : MonoSingleton<T> where T : UI_DebugBase<T>
    {
        private const bool disableALLUI = false;
        [SerializeField] private bool active = true;
        private bool IsActivated => active && !disableALLUI;
        [SerializeField] private List<TextMeshProUGUI> list;
        public IList<TextMeshProUGUI> GetList => list;
        protected virtual void Start()
        {
            gameObject.SetActive(IsActivated);
        }
    }
}
