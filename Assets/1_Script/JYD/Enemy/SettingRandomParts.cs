using UnityEngine;

namespace Swift_Blade.Enemy
{
    public class SettingRandomParts : MonoBehaviour
    {
        [Header("Weapon Info")] 
        public string weaponName;
        public GameObject[] weapons;
        
        [Header("Head Info")]
        public GameObject[] heads;
        
        [Header("Helmet Info")]
        public GameObject[] helmets;
        
        [Header("Chest Info")]
        public GameObject[] Chests;
        
        [Header("RightArms Info")]
        public GameObject[] RightArms;
        
        [Header("LeftArms Info")]
        public GameObject[] LeftArms;
        
        [Header("Right Shoulder Info")]
        public GameObject[] RightShoulders;
        
        [Header("Left Shoulder Info")]
        public GameObject[] LeftShoulders;
                
        [Header("Pants Info")]
        public GameObject[] Pants;
        
        [Header("RightLeg Info")]
        public GameObject[] RightLegs;
        
        [Header("LeftLeg Info")]
        public GameObject[] leftLegs;
                

        private BaseEnemyAnimationController animatorController;
        private BaseEnemy enemy;
        
        private void Start()
        {
            animatorController = GetComponent<BaseEnemyAnimationController>();
            enemy = GetComponent<BaseEnemy>();
            
            SetRandomPart(heads);
            SetRandomPart(helmets);
            SetRandomPart(Chests);
            SetRandomPart(RightArms,LeftArms);
            SetRandomPart(LeftShoulders,RightShoulders);
            SetRandomPart(Pants);
            SetRandomPart(leftLegs, RightLegs);
            
            SetRandomWeapon();
        }
        
        private void SetRandomPart(GameObject[] parts)
        {
            if(parts.Length == 0)return;
            
            foreach (var part in parts)
            {
                part.SetActive(false);
            }
            
            int randIndex = Random.Range(0, parts.Length);
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
                        
            int randIndex = Random.Range(0, parts1.Length != 0 ? parts1.Length : parts2.Length != 0 ? parts2.Length : 0);
            if (parts1.Length != 0)
            {
                parts1[randIndex].SetActive(true);
            }

            if (parts2.Length != 0)
            {
                parts2[randIndex].SetActive(true);
            }
            
        }
        
        private void SetRandomWeapon()
        {
            foreach (var weapon in weapons)
            {
                weapon.SetActive(false);
                weapon.gameObject.name = "No";
            }
            
            int randIndex = Random.Range(0, weapons.Length);
            enemy.weapon = weapons[randIndex];
            enemy.weapon.SetActive(true);
            enemy.weapon.name = weaponName;
            
            animatorController.Rebind();
                        
        }
    }
}
