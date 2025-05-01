using UnityEngine;
using DG.Tweening;

namespace Swift_Blade.Pool
{
    public class DragonBall : MonoBehaviour, IPoolable
    {
        [SerializeField] private LayerMask _whatIsEnemy;
        [SerializeField] private Transform _particle;

        private float _damage;
        private float _originAngle;

        private SphereCollider _collider;

        private Sequence _sequence;

        public void Initialize(float damage, float angle)
        {
            _damage = damage;
            _originAngle = angle;
            transform.localEulerAngles = Vector3.up * angle;
        }

        public void OnPop()
        {
            _collider = GetComponent<SphereCollider>();
            
            _sequence = DOTween.Sequence();
            
            _collider.enabled = false;
            _particle.gameObject.SetActive(false);
        }

        public void Enable()
        {
            _collider.enabled = true;
            _particle.gameObject.SetActive(true);
            
            _sequence = DOTween.Sequence();

            _sequence.Append(DOTween.To(() => _collider.center.z, x => _collider.center = Vector3.forward * x, 1f, 1f));
            _sequence.Join(_particle.DOLocalMoveZ(1f, 1f));
            _sequence.Append(transform.DOLocalRotate(Vector3.up * (_originAngle + 1080), 2.8f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(10, LoopType.Restart));

            _sequence.Append(DOTween.To(() => _collider.center.z, x => _collider.center = Vector3.forward * x, 0f, 1f));
            _sequence.Join(_particle.DOLocalMoveZ(0f, 1f));
            _sequence.AppendCallback(() => {
                _collider.enabled = false;
                _particle.gameObject.SetActive(false);
            });
            _sequence.AppendInterval(6f);
            _sequence.AppendCallback(Enable);
        }

        public void Disable()
        {
            _sequence.Kill();
            
            _collider.enabled = false;
            _particle.gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(((1 << other.gameObject.layer) & _whatIsEnemy.value) > 0)
            {
                if(other.TryGetComponent(out IHealth health))
                {
                    ActionData actionData = new ActionData(transform.position, Vector3.up, _damage, false);
                    health.TakeDamage(actionData);
                    FloatingTextGenerator.Instance.GenerateText(Mathf.RoundToInt(_damage).ToString(), other.transform.position);
                }
            }
        }
    }
}
