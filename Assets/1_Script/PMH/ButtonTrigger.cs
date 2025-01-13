using DG.Tweening;
using UnityEngine;

namespace Swift_Blade
{
    public class ButtonTrigger : MonoBehaviour
    {
        [SerializeField] private Transform CurrentDoor;

        private bool canTrigger = false;

        private bool doorActive = false;

        private void Update()
        {
            if(canTrigger)
            {
                if(Input.GetKeyDown(KeyCode.F))
                {
                    DoorOpenTrigger();
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                Debug.Log("범위 안에 들아오다");
                canTrigger = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(canTrigger)
                canTrigger = false;
        }

        private void DoorOpenTrigger()
        {
            Debug.Log("문작동");

            //카메라는 문을 포커싱

            doorActive = !doorActive;

            if (doorActive)
            {
                CurrentDoor.DOMoveY(CurrentDoor.position.y - 3, 0.5f);
            }
            else
            {
                CurrentDoor.DOMoveY(CurrentDoor.position.y + 3, 0.5f);
            }

            
           
        }
    }
}
