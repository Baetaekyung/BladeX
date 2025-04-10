using UnityEngine;
using DG.Tweening;
using Swift_Blade.Pool;

namespace Swift_Blade.Level
{
    public enum ChestType //에 따라서 나올 아이템 확률 조작? (예정)
    {
        Bronze,
        Silver,
        Gold
    }

    public class Chest : MonoBehaviour, IInteractable
    {
        private Transform playerTrm;

        //[SerializeField] private float shootAngle = -15f;
        //[SerializeField] private Vector2 shootPower;

        [SerializeField] private BaseOrb orb;
        [SerializeField] private ItemTableSO itemTableSO;

        [SerializeField] private PoolPrefabMonoBehaviourSO shinyParticlePrefab;
        [SerializeField] private Transform shinyParticleTrm;

        [SerializeField] private Transform[] visuals;

        private Transform chestLid;
        private ChestType chestType;
        private new Rigidbody rigidbody;

        private bool isOpen = false;

        private void Start()
        {
            MonoGenericPool<ShinyParticle>.Initialize(shinyParticlePrefab);
            rigidbody = GetComponent<Rigidbody>();

            SetRandomChestType();

            Vector3 lookDirection = (Camera.main.transform.forward - transform.position).normalized;
            lookDirection.y = 0;

            transform.rotation = Quaternion.LookRotation(lookDirection);

        }

        private void SetRandomChestType()
        {
            for (int i = 0; i < visuals.Length; i++)
            {
                visuals[i].gameObject.SetActive(false);
            }

            int n = Random.Range(0, 3);
            chestType = (ChestType)n;
            visuals[n].gameObject.SetActive(true);
            chestLid = visuals[n].GetChild(0);

        }
        private void OpenChest()
        {
            if (isOpen) return;
            isOpen = true;

            gameObject.layer = LayerMask.NameToLayer("Default");

            OpenChestAnimations();
            InstItemOrb();
            GetRandomItem();
            ShinyParticle shinyParticle = MonoGenericPool<ShinyParticle>.Pop();
            shinyParticle.transform.position = shinyParticleTrm.position;
        }
        private void OpenChestAnimations()
        {
            Vector3 openLidAngle = new Vector3(-90, transform.eulerAngles.y, transform.eulerAngles.z);
            chestLid.DORotate(openLidAngle, 0.3f)
                .SetEase(Ease.OutQuad);

            rigidbody.DOJump(rigidbody.position + Vector3.up * 0.35f, 0.1f, 1, 0.15f)
                .SetEase(Ease.OutExpo)
                .SetDelay(0.3f);
            rigidbody.DORotate(rigidbody.rotation.eulerAngles + new Vector3(5, 0, 0), 0.1f);
        }

        private void InstItemOrb()
        {
            Vector3 spawnPos = transform.localPosition + new Vector3(0, 0.7f, 0);
            BaseOrb orbInstance = Instantiate(orb, spawnPos, Quaternion.identity);
            orbInstance.SetColor((ColorType)Random.Range(0, 3));
            
            //orbInstance.transform.DOMoveY(transform.position.y + 3f, 0.4f)
            //    .SetEase(Ease.InBack)
            //    .SetLink(orbInstance.gameObject, LinkBehaviour.KillOnDestroy);

        }
        private ItemDataSO GetRandomItem()
        {
            int itemCount = itemTableSO.itemTable.Count;
            int randomIndex = Random.Range(0, itemCount);

            return itemTableSO.itemTable[randomIndex].itemData;

            //InventoryManager.Instance.AddItemToEmptySlot(item.itemData);
            //인벤토리SO에 add엠티
        }

        public void Interact()
        {
            //Debug.Log("열려라참깨");
            OpenChest();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerTrm = other.transform;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (playerTrm != null && other.CompareTag("Player"))
            {
                playerTrm = null;
            }
        }
    }
}
