using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

namespace Swift_Blade
{
    public class BossRoomEnterTrigger : MonoBehaviour,ITriggerable
    {
        public bool trigger = false;

        public PlayableDirector cutScene;
                
        public void Trigger()
        {
            if(IsTriggered())return;
            
            trigger = true;
            cutScene.Play();
        }

        public bool IsTriggered()
        {
            return trigger;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Trigger();
            }
        }
        
    }
}
