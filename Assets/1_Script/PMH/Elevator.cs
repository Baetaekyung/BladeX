using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace Swift_Blade
{
    public class Elevator : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform PlatformTrm;
        [SerializeField] private bool IsPlayerOnthePanel = false;
        [SerializeField] private bool IsDowned = true;

        [SerializeField] private float flatTime = 2.0f;

        [SerializeField] private float onBoardTime = 3.0f; //�÷��̾ �� �ð����� �÷��� ���� ������ �ڵ����� ������.
        [SerializeField] private bool isMoving = false;

        [SerializeField] private float upFloorHeight, downFloorHeight;
        [SerializeField] private float liftDuration = 2f;

        [SerializeField] private Collider coll;

        private void SetPlatUpdown()
        {
            IsDowned = !IsDowned;
            //coll.isTrigger = !IsDowned;

            if (IsDowned)
            {
                Debug.Log("��������");
                PlatformTrm.DOMoveY(upFloorHeight, liftDuration);
                StartCoroutine("CheckThisMoveCoroutine");
            }
            else
            {
                Debug.Log("�����������ٴ�����");
                PlatformTrm.DOMoveY(downFloorHeight, liftDuration);
                StartCoroutine("CheckThisMoveCoroutine");
                
            }

        }

        private IEnumerator CheckThisMoveCoroutine()
        {
            isMoving = true;
            yield return new WaitForSeconds(liftDuration);
            isMoving = false;

            yield return new WaitForSeconds(onBoardTime);
            PlatformTrm.DOMoveY(downFloorHeight, liftDuration);

            IsDowned = false;
            isMoving = true;
            yield return new WaitForSeconds(liftDuration);
            isMoving = false;
        }
        private void OnCollisionEnter(Collision collision)
        {
            if(collision.transform.CompareTag("Player"))
            {
                IsPlayerOnthePanel = true;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                IsPlayerOnthePanel = false;

                onBoardTime = flatTime;
            }
        }

        public void Interact()
        {
            if (IsPlayerOnthePanel == false) return;
            if (isMoving) return;
            SetPlatUpdown();
        }
    }
}
