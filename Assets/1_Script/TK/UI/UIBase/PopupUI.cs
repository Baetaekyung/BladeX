using System;
using UnityEngine;

public enum PopupType
{
    Option,
    Help
}

namespace Swift_Blade.UI
{
    [Serializable]
    public abstract class PopupUI : MonoBehaviour
    {
        public abstract void Popup();
        public abstract void PopDown();
    }
}
