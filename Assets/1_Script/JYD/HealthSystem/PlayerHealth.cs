using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;

namespace Swift_Blade
{
    public class PlayerHealth : MonoBehaviour,IDamageble, IEntityComponentRequireInit
    {
        private Player _player;
        [SerializeField] private StatComponent _statCompo;
        [SerializeField] private StatSO _healthStat;

        private float _maxHealth;
        private float _currentHealth;

        public event Action OnDeadEvent;
        public event Action OnHitEvent;


        [Header("Flash info")]
        [SerializeField] private float flashDuration;
        [SerializeField] private Material _flashMat;
        [SerializeField] private SkinnedMeshRenderer[] _meshRenderers;
        private Material[] _originMats;
        
        private void Start()
        {
            currentHealth = maxHealth;
            
            _meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            _originMats = new Material[_meshRenderers.Length];
            for (int i = 0; i < _meshRenderers.Length; i++)
            {
                _originMats[i] = _meshRenderers[i].material;
            }

            OnHitEvent += FlashMat;
        }
        
        private void OnDestroy()
        {
            OnHitEvent -= FlashMat;
        }

        public void TakeDamage(ActionData actionData)
        {
            float damageAmount = actionData.damageAmount;
            currentHealth -= damageAmount;
            
            OnHitEvent?.Invoke();
            
            if (currentHealth <= 0)
                Dead();
        }

        public void TakeHeal()
        {
            //이거 매개변수 없으면 안되는거 아닌가?
        }

        public void TakeHeal(float healAmount) //힐 받으면 현재 체력에 HealAmount 더한 값으로 변경
        {
            _currentHealth = Mathf.Clamp(_currentHealth + healAmount, 0, _maxHealth);
        }

        public void Dead()
        {
            OnDeadEvent?.Invoke();
            //Debug.Log("플레이어 죽었슴");
        }
        
        private void FlashMat()
        {
            StartCoroutine(FlashRoutine());
        }
    
        private IEnumerator FlashRoutine()
        {
            foreach (var t in _meshRenderers)
            {
                t.material = _flashMat;
            }
        
            yield return new WaitForSeconds(flashDuration);

            for (int i = 0; i < _meshRenderers.Length; i++)
            {
                _meshRenderers[i].material = _originMats[i];
            }
        }

        /// <param name="value">추가할 체력 값</param>
        public void AddBaseHealth(float value) //기본 값 변경이라 키 값이 필요 없음.
        {
            _statCompo.SetBaseValue(_healthStat, _maxHealth + value);
            _maxHealth = _healthStat.Value;
            _currentHealth = Mathf.Clamp(_currentHealth + value, 0, _healthStat.Value);
        }
    }
}
