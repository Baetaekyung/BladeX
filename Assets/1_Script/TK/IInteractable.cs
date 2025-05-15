using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Swift_Blade
{
    public interface IInteractable
    {
        public void Interact();
        public bool IsHurtWhenInteracting() => false;
        public GameObject GetMeshGameObject()
        {
            return null;
        }
        //public Material GetHighlightMaterial();
        //public void OnEndCallbackSubscribe(Action onEndCallback) { }
        //public void OnEndCallbackUnsubscribe(Action onEndCallback) { }
    }
}
