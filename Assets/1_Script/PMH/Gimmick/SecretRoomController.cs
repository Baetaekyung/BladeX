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
            /// 비밀의 방으로 이동
            /// 이동할때 카메라를 비밀의 방으로 이동
            /// 카메라 셰이크
            /// 비밀의 방, 또는 비밀의 섬이 안개 밑에서 올라옴

            Sequence seq = DOTween.Sequence();

            Vector3 pos = secretRoomTrm.position + new Vector3(0, Mathf.Abs( secretRoomTrm.localPosition.y ), 0);
            Vector3 bridgePos = secretRoomBridgeTrm.position + new Vector3(0, Mathf.Abs(secretRoomBridgeTrm.localPosition.y), 0);

            CameraShakeManager.Instance.DoShake(CameraShakeType.Middle);
            seq.Append(secretRoomTrm.DOMove(pos, 1f)); //비밀의 방 올라옴

            CameraShakeManager.Instance.DoShake(CameraShakeType.Middle);
            seq.Append(secretRoomBridgeTrm.DORotate(bridgePos, 1f)); //연결다리 올라옴
        }
    }
}
