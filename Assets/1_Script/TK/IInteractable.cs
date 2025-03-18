using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Swift_Blade
{
    public interface IInteractable
    {
        public void Interact();
        public void OnEndCallbackSubscribe(Action onEndCallback) { }
        public void OnEndCallbackUnsubscribe(Action onEndCallback) { }
    }
}
