using UnityEngine;
using DG.Tweening;
using Swift_Blade.Feeling;

namespace Swift_Blade
{
    public class SecretRoomController : MonoBehaviour,IInteractable
    {
        [SerializeField] private Transform secretRoomTrm;
        [SerializeField] private Transform secretRoomBridgeTrm;

        private bool isActive = false;

        public void Interact()
        {
            if (isActive) return;

            isActive = true;
            /// ����� ������ �̵�
            /// �̵��Ҷ� ī�޶� ����� ������ �̵�
            /// ī�޶� ����ũ
            /// ����� ��, �Ǵ� ����� ���� �Ȱ� �ؿ��� �ö��

            Sequence seq = DOTween.Sequence();

            Vector3 pos = secretRoomTrm.position + new Vector3(0, Mathf.Abs( secretRoomTrm.localPosition.y ), 0);
            Vector3 bridgePos = secretRoomBridgeTrm.position + new Vector3(0, Mathf.Abs(secretRoomBridgeTrm.localPosition.y), 0);

            CameraShakeManager.Instance.DoShake(CameraShakeType.Middle);
            seq.Append(secretRoomTrm.DOMove(pos, 1f)); //����� �� �ö��

            CameraShakeManager.Instance.DoShake(CameraShakeType.Middle);
            seq.Append(secretRoomBridgeTrm.DORotate(bridgePos, 1f)); //����ٸ� �ö��
        }
    }
}
