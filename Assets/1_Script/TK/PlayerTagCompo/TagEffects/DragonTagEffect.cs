using UnityEngine;
using Swift_Blade.Pool;

namespace Swift_Blade
{
    public class DragonTagEffect : TagEffectBase
    {
        [SerializeField] private float _damage;
        [SerializeField] private PoolPrefabMonoBehaviourSO _dragonBall;

        private Transform _playerTransform;

        private DragonBall[] _dragonBalls;

        public override void Initialize(Player player)
        {
            base.Initialize(player);

            _playerTransform = player.transform.GetChild(0);

            MonoGenericPool<DragonBall>.Initialize(_dragonBall);
            
            _dragonBalls = new DragonBall[3];
            for(int i = 0; i < 3; ++i)
            {
                DragonBall dragonBall = MonoGenericPool<DragonBall>.Pop();
                dragonBall.Initialize(_damage, 120 * i);

                dragonBall.transform.SetParent(_playerTransform);
                dragonBall.transform.localPosition = Vector3.zero;

                _dragonBalls[i] = dragonBall;
            }
        }

        protected override void TagEnableEffect(int tagCount)
        {
            for(int i = 0; i < 3; ++i)
            {
                _dragonBalls[i].Enable();
            }
        }

        protected override void TagDisableEffect()
        {
            for(int i = 0; i < 3; ++i)
            {
                _dragonBalls[i].Disable();
            }
        }
    }
}
