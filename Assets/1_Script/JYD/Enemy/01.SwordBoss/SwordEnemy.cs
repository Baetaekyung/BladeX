using System;
using Swift_Blade.Enemy.Boss.Sword;
using UnityEngine;

namespace Swift_Blade.Enemy.Sword
{
    public class SwordEnemy : SwordBoss
    {
        [Header("Weapon Info")] 
        public string swordName;
        public GameObject[] swords;
        
        [Header("Head Info")]
        public GameObject[] heads;
        
        [Header("Chest Info")]
        public GameObject[] Chests;
        
        [Header("Left Shoulder Info")]
        public GameObject[] LeftShoulders;
        
        [Header("Right Shoulder Info")]
        public GameObject[] RightShoulders;
        
        protected override void Start()
        {
            base.Start();
            
            SetRandomPart(heads);
            SetRandomPart(Chests);
            SetRandomPart(LeftShoulders,RightShoulders);
            
            SetRandomSword();
        }

        private void SetRandomSword()
        {
            foreach (var sword in swords)
            {
                sword.SetActive(false);
                sword.gameObject.name = "No";
            }
            
            int randIndex = UnityEngine.Random.Range(0, swords.Length);
            sword = swords[randIndex];
            
            sword.SetActive(true);
            sword.gameObject.name = swordName;    
            
            (baseAnimationController as SwordEnemyAnimationController).Rebind();
        }

        private void SetRandomPart(GameObject[] parts)
        {
            foreach (var part in parts)
            {
                part.SetActive(false);
            }
            
            int randIndex = UnityEngine.Random.Range(0, parts.Length);
            parts[randIndex].SetActive(true);
        }
        
        private void SetRandomPart(GameObject[] parts1,GameObject[] parts2)
        {

            foreach (var part in parts1)
            {
                part.SetActive(false);
            }
            
            foreach (var part in parts2)
            {
                part.SetActive(false);
            }
            
            int randIndex = UnityEngine.Random.Range(0, parts1.Length);
            parts1[randIndex].SetActive(true);
            parts2[randIndex].SetActive(true);
        }
    }
}