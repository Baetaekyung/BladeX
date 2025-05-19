using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Swift_Blade.Pool;

namespace Swift_Blade
{
    public class ShieldEffect : MonoBehaviour
    {
        [SerializeField] [Range(0.1f, 300)] private float rotateSpeed;
        [SerializeField] private PoolPrefabMonoBehaviourSO hexagonParticle;
        
        private List<Material> shieldMats;
        
        private const string TINT_COLOR = "_TintColor";
        private const float MAX_ALPHA_VALUE = 0.4f;
        
        private int _currentShieldAmount = 0;
        
        private void Awake()
        {
            shieldMats = GetComponentsInChildren<MeshRenderer>().Select(x => x.material).ToList();
        }
        
        private void Update()
        {
            if(_currentShieldAmount == 0) return;
            
            transform.Rotate(Vector3.up * (rotateSpeed * Time.deltaTime));
        }

        public void SetShield(int amount)
        {
            if(amount == _currentShieldAmount) return;
            
            float angle = 360f / amount;
            for(int i = 0; i < amount; ++i)
            {
                transform.GetChild(i).localEulerAngles = Vector3.up * (angle * i);
            }

            if(amount > _currentShieldAmount)
            {
                AddShield(amount);
            }
            else
            {
                BreakShield(amount);
            }

            _currentShieldAmount = amount;
        }

        private void AddShield(int amount)
        {
            MonoGenericPool<HexagonParticle>.Initialize(this.hexagonParticle);
            
            HexagonParticle hexagon = MonoGenericPool<HexagonParticle>.Pop();
            hexagon.SetFollowTransform(transform);
            
            for(int i = 0; i < amount; ++i)
            {
                CompleteFade(shieldMats[i] , MAX_ALPHA_VALUE);
            }
            
        }
        
        private void BreakShield(int amount)
        {
            int count = shieldMats.Count;
            for (int i = amount; i < count; i++)
            {
                CompleteFade(shieldMats[i] , 0f);
            }
        }

        private void CompleteFade(Material material, float endValue)
        {
            Color c = material.GetColor(TINT_COLOR);
            c.a = endValue;
            material.SetColor(TINT_COLOR, c);
        }
        
    }
}
