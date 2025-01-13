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
                Debug.Log("���� �ȿ� ��ƿ���");
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
            Debug.Log("���۵�");

            //ī�޶�� ���� ��Ŀ��

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
