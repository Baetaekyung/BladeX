using System;
using UnityEngine;

namespace Swift_Blade
{
    public class ListenerBase : MonoBehaviour
    {
        public event Action DefaultFireEvent;
        public void FireBaseAction()
        {
            DefaultFireEvent?.Invoke();
        }
    }
}
