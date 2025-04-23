using System;
using Swift_Blade.Pool;
using UnityEngine;
using DG.Tweening;

namespace Swift_Blade.Level.Door
{
    public class Door : MonoBehaviour, IInteractable
    {
        [SerializeField] private SceneManagerSO sceneManager;
        [SerializeField] private NodeList nodeList;
        [SerializeField] private PoolPrefabMonoBehaviourSO dustPrefab;
        
        [SerializeField] private bool isDefaultPortal;
        
        [Range(0.1f , 10)] [SerializeField] private float enterDelay;
        [Range(0.1f , 10)] [SerializeField] private float enterDuration;
        [Range(0.1f , 2)] [SerializeField] private float cageDownDuration;
        
        [Space]
        
        [SerializeField] private Transform door;
        [SerializeField] private Transform cage;
        [SerializeField] private string sceneName;

        private void Awake()
        {
            MonoGenericPool<DustUpParticle>.Initialize(dustPrefab);   
        }

        private void Start()
        {
            if (isDefaultPortal)
            {
                SetScene(nodeList.GetNodeName(NodeType.Exp));
            }
                       
            //DOVirtual.DelayedCall(delay, (UpDoor()));
        }

        private void Rotate()
        {
            Vector3 direction = Camera.main.transform.position - door.position;
            direction.y = 0; 
            door.rotation = Quaternion.LookRotation(-direction);
        }
               
        /*private void OnEnable()
        {
            cinemachineCamera.Priority = 1;
            DOVirtual.DelayedCall(delay, UpDoor);
        }

        private void OnDisable()
        {
            door.transform.position -= new Vector3(0,2.6f , 0);
        }*/
        
        /*public IEnumerator UpDoor(float doorMoveDelay = 0)
        {
            bool isFinished = false;
            
            cinemachineCamera.Priority = 1;
            
            Sequence sequence = DOTween.Sequence();
            sequence.AppendCallback(() => CameraShakeManager.Instance.DoShake(cameraShakeType));
            sequence.Join(door.DOMoveY(transform.position.y + 0.25f, duration));
            sequence.AppendInterval(doorMoveDelay);
            sequence.AppendCallback(() => cinemachineCamera.Priority = -1);
            sequence.OnComplete(() => isFinished = true); 

            yield return new WaitUntil(() => isFinished);
        }*/
        
        public void SetScene(string _sceneName)
        {
            sceneName = _sceneName;
        }
        
                
        public void UpDoor()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(enterDelay);
            sequence.AppendCallback(Rotate);
            sequence.Append(door.DOMoveY(transform.position.y + 0.25f, enterDuration));
            sequence.OnComplete(() =>
            {
                var isFinished = true;
            });
            
            DustUpParticle dustParticle = MonoGenericPool<DustUpParticle>.Pop();
            dustParticle.transform.position = transform.position;
        }
        
        public void Interact()
        {
            cage.transform.DOLocalMoveY(-2.25f ,cageDownDuration ).SetEase(Ease.OutQuart);
            sceneManager.LoadScene(sceneName);
        }

       
        
    }
}
