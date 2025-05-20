using Random = UnityEngine.Random;
using System.Collections.Generic;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Skill
{
    public abstract class SkillData : ScriptableObject, IPlayerEquipable
    {
        public string skillName;
        public Sprite skillIcon;
        [Space(30)]
        public SkillType skillType;
        public StatType statType;
        public ColorType colorType;
        ColorType IPlayerEquipable.GetColor => colorType;
        Sprite IPlayerEquipable.GetSprite => skillIcon;
        string IPlayerEquipable.DisplayName => skillName;


        [Tooltip("비율")] public float colorRatio;
        [Tooltip("확률")][Range(1,100)] public int random;
        [Tooltip("이거보다 높아질순 없음")][Range(1,100)] public int maxRandom;
        
        [TextArea] public string skillDescription;
        
        [Space(40)]
        public PoolPrefabMonoBehaviourSO skillParticle;

        protected PlayerStatCompo statCompo;
        
        private string activatedInfoString;
        private string deactivatedInfoString;
        
        
        public void SetPlayerStatCompo(PlayerStatCompo playerStatCompo)
        {
            statCompo = playerStatCompo;
        }

        private void OnEnable()
        {
            string particle = GetSubjectParticle(skillName);
            string colorCode = GetColorCode();
            
            activatedInfoString = $"<color={colorCode}>{skillName}</color>{particle} 발동되었습니다";
            deactivatedInfoString = $"<color={colorCode}>{skillName}</color>{particle} 해제되었습니다";
        }

        public virtual void Initialize()
        {
            
        }

        public virtual void ResetSkill()
        {
            
        }
        
        public virtual void Render(){}
        
        public virtual void SkillUpdate(Player player, IEnumerable<Transform> targets = null){}
        
        public abstract void UseSkill(Player player, IEnumerable<Transform> targets = null);
        
        protected bool TryUseSkill(int value = 0)
        {
            return Random.Range(0, 100) <= Mathf.Min(maxRandom, random + value);
        }
        
        protected float GetColorRatio()
        {
            if (statCompo == null)
            {
                Debug.LogError("Skill Error : statCompo is null");
                return 0;
            }
            
            return statCompo.GetColorStatValue(colorType) * colorRatio;
        }
        private static string GetSubjectParticle(string word)
        {
            char lastChar = word[word.Length - 1];
            bool hasBatchim = (lastChar - 0xAC00) % 28 != 0;
            return hasBatchim ? "이" : "가";
        }
        
        private string GetColorCode()
        {
            return colorType switch
            {
                ColorType.RED => "#FF4444",
                ColorType.GREEN => "#44FF44",
                ColorType.BLUE => "#4444FF",
                ColorType.TURQUOISE => "#44FFFF",
                ColorType.PURPLE => "#AA44FF",
                ColorType.YELLOW => "#FFFF44",
                _ => "#FFFFFF"
            };
        }
                    
        private string GetInfoString(bool active)
        {
            return active ? activatedInfoString : deactivatedInfoString;
        }
        
        protected void GenerateSkillText(bool active)
        {
            string infoString = GetInfoString(active);
            PopupManager.Instance.LogInfoBox(infoString);
        }
        
  
    }
}