using UnityEngine;
using DG.Tweening;
using Swift_Blade.Feeling;

namespace Swift_Blade
{
    public class SecretRoomController : MonoBehaviour,IInteractable
    {
        [SerializeField] private Transform secretRoomTrm;
        [SerializeField] private Transform secretRoomBridgeTrm;

        [SerializeField] private Transform offBoundery;

        private bool isActive = false;

        public void Interact()
        {
            if (isActive) return;

            isActive = true;
            /// ����� ������ �̵�
            /// �̵��Ҷ� ī�޶� ����� ������ �̵�
            /// ī�޶� ����ũ
            /// ����� ��, �Ǵ� ����� ���� �Ȱ� �ؿ��� �ö��

            if(offBoundery != null)
            {
                offBoundery.gameObject.SetActive(false);
            }

            Sequence seq = DOTween.Sequence();

            if(secretRoomTrm != null)
            {
                Vector3 pos = secretRoomTrm.position + new Vector3(0, Mathf.Abs(secretRoomTrm.localPosition.y), 0);
                CameraShakeManager.Instance.DoShake(CameraShakeType.Middle);
                seq.Append(secretRoomTrm.DOMove(pos, 1f)); //����� �� �ö��
            }
            
            if(secretRoomBridgeTrm != null)
            {
                Vector3 bridgePos = secretRoomBridgeTrm.position + new Vector3(0, Mathf.Abs(secretRoomBridgeTrm.localPosition.y), 0);
                CameraShakeManager.Instance.DoShake(CameraShakeType.Middle);
                seq.Append(secretRoomBridgeTrm.DOMove(bridgePos, 1f)); //����ٸ� �ö��
                Debug.Log("����");
            }
        }
    }
}
