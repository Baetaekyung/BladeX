using DG.Tweening;
using UnityEngine;

namespace Swift_Blade
{
    public enum TriggerType
    {
        Door,
        Ground
    }

    public class ButtonTrigger : MonoBehaviour
    {
        [SerializeField] private TriggerType type;
        [SerializeField] private Transform targetPos;

        [SerializeField] private Transform CurrentObj;

        private bool canTrigger = false;

        private bool doorActive = false;

        private void Update()
        {
            if(canTrigger)
            {
                if(Input.GetKeyDown(KeyCode.F))
                {
                    switch (type)
                    {
                        case TriggerType.Door:
                            DoorOpenTrigger();
                            break;
                        case TriggerType.Ground:
                            GroundMoveTrigger();
                            break;
                    }

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
                CurrentObj.DOMoveY(CurrentObj.position.y - 3, 0.5f);
            }
            else
            {
                CurrentObj.DOMoveY(CurrentObj.position.y + 3, 0.5f);
            }  
        }

        private void GroundMoveTrigger()
        {
            Debug.Log(CurrentObj.position);
            Debug.Log(targetPos.position);
            float dist = Vector3.Distance(CurrentObj.position, targetPos.position);
            CurrentObj.DOMoveY(targetPos.position.y, dist * 0.5f);
        }
    }
}
