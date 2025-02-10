using System;
using UnityEngine;

namespace Swift_Blade
{
    public class EventTester : MonoBehaviour
    {
        [SerializeField] private EquipmentChannelSO channel;

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.P))
                channel.RaiseEvent();
        }
    }
}
