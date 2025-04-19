using UnityEngine;
using DG.Tweening;
using Swift_Blade.Pool;
using System;
using Random = UnityEngine.Random;

namespace Swift_Blade.Level
{
    public enum ChestType
    {
        Bronze,
        Silver,
        Gold,
    }

    public class Chest : MonoBehaviour, IInteractable
    {
        [Header("Prelace")]
        [SerializeField] private bool prePlaced;
        [SerializeField] private ChestType chestType;

        [Header("Settings")]
        [SerializeField] private BaseOrb orbItem;
        [SerializeField] private BaseOrb orbWeapon;
        [SerializeField] private BaseOrb orbSkill;

        [SerializeField] private PoolPrefabMonoBehaviourSO shinyParticlePrefab;
        [SerializeField] private Transform shinyParticleTrm;

        [SerializeField] private Transform[] visuals;

        //private Transform playerTrm;
        private Transform chestLid;
        private new Rigidbody rigidbody;

        private bool isOpen = false;

        private void Awake()
        {
            MonoGenericPool<ShinyParticle>.Initialize(shinyParticlePrefab);
            rigidbody = GetComponent<Rigidbody>();

            if (prePlaced)
            {
                SetChest(chestType);
            }
            else
            {
                SetRandomChestType();
            }

            //Vector3 lookDirection = (Camera.main.transform.forward - transform.position).normalized;
            //lookDirection.y = 0;
            //
            //transform.rotation = Quaternion.LookRotation(lookDirection);
        }
        private void SetChest(ChestType chestType)
        {
            int visualsLength = visuals.Length;
            for (int i = 0; i < visualsLength; i++)
            {
                visuals[i].gameObject.SetActive(false);
            }

            this.chestType = chestType;
            int index = (int)chestType;
            Transform chestTransform = visuals[index];
            chestTransform.gameObject.SetActive(true);
            chestLid = chestTransform.GetChild(0);
        }
        private void SetRandomChestType()
        {
            int randomIndex = Random.Range(0, visuals.Length);
            ChestType randomChestType = (ChestType)randomIndex;
            SetChest(randomChestType);
        }
        private void OpenChest()
        {
            if (isOpen) return;
            isOpen = true;

            gameObject.layer = LayerMask.NameToLayer("Default");

            OpenChestAnimations();
            InstansiateItemOrb();
            //GetRandomItem();
            ShinyParticle shinyParticle = MonoGenericPool<ShinyParticle>.Pop();
            shinyParticle.transform.position = shinyParticleTrm.position;
        }
        private void OpenChestAnimations()
        {
            Vector3 openLidAngle = new Vector3(-120, transform.eulerAngles.y, transform.eulerAngles.z);
            chestLid.DORotate(openLidAngle, 0.3f)
                .SetEase(Ease.OutQuad);

            rigidbody.DOJump(rigidbody.position + Vector3.up * 0.35f, 0.1f, 1, 0.15f)
                .SetEase(Ease.OutExpo)
                .SetDelay(0.3f);
            rigidbody.DORotate(rigidbody.rotation.eulerAngles + new Vector3(5, 0, 0), 0.1f);
        }
        private void InstansiateItemOrb()
        {
            Vector3 spawnPos = transform.localPosition + new Vector3(0, 0.7f, 0);
            BaseOrb orbTarget = GetOrb(chestType);
            Instantiate(orbTarget, spawnPos, Quaternion.identity);
            //orbInstance.SetRandom();// (ColorType)Random.Range(0, 3));

            //orbInstance.transform.DOMoveY(transform.position.y + 3f, 0.4f)
            //    .SetEase(Ease.InBack)
            //    .SetLink(orbInstance.gameObject, LinkBehaviour.KillOnDestroy);
        }
        private BaseOrb GetOrb(ChestType chestType)
        {
            BaseOrb result = chestType switch
            {
                ChestType.Bronze => orbItem,
                ChestType.Silver => orbSkill,
                ChestType.Gold => orbWeapon,
                _ => throw new ArgumentOutOfRangeException($"{chestType} is not available")
            };
            return result;
        }
        void IInteractable.Interact()
        {
            OpenChest();
        }

        //private void OnTriggerEnter(Collider other)
        //{
        //    if (other.CompareTag("Player"))
        //    {
        //        playerTrm = other.transform;
        //    }
        //}
        //private void OnTriggerExit(Collider other)
        //{
        //    if (playerTrm != null && other.CompareTag("Player"))
        //    {
        //        playerTrm = null;
        //    }
        //}
    }
}
