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

        [SerializeField] private float onBoardTime = 3.0f; //플레이어가 이 시간동안 플랫폼 위에 없으면 자동으로 내려감.
        [SerializeField] private bool IsMoving = false;

        [SerializeField] private float upFloorHeight, downFloorHeight;
        [SerializeField] private float liftDuration = 2f;

        private void Update()
        {

        }
        private void SetPlatUpdown()
        {
            IsDowned = !IsDowned;

            if (IsDowned)
            {
                PlatformTrm.DOMoveY(upFloorHeight, liftDuration);
                StartCoroutine("CheckThisMoveCoroutine");
            }
            else
            {
                PlatformTrm.DOMoveY(downFloorHeight, liftDuration);
                StartCoroutine("CheckThisMoveCoroutine");
            }

        }

        private IEnumerator CheckThisMoveCoroutine()
        {
            IsMoving = true;
            yield return new WaitForSeconds(liftDuration);
            IsMoving = false;

            yield return new WaitForSeconds(onBoardTime);
            PlatformTrm.DOMoveY(downFloorHeight, liftDuration);
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
            if (IsMoving) return;
            SetPlatUpdown();
        }
    }
}
