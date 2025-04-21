using DG.Tweening;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade
{
    public class FloatingTextGenerator : MonoSingleton<FloatingTextGenerator>
    {
        [SerializeField] private PoolPrefabMonoBehaviourSO floatingTextPoolSO;

        protected override void Awake()
        {
            base.Awake();

            MonoGenericPool<FloatingText>.Initialize(floatingTextPoolSO);
        }

        public void GenerateText(string message, Vector3 position, Color color)
        {
            FloatingText text = GenerateText(position);
            text.SetText(message, color);
            text.Animation();

            text.OnComplete += ()
                => MonoGenericPool<FloatingText>.Push(text);
        }

        //Just call genereateText method
        public void GenerateText(string message, Vector3 position)
        {
            FloatingText text = GenerateText(position);
            text.SetText(message);
            text.Animation();

            text.OnComplete += ()
                => MonoGenericPool<FloatingText>.Push(text);
        }

        private FloatingText GenerateText(Vector3 position)
        {
            var floatingText = MonoGenericPool<FloatingText>.Pop();

            floatingText.transform.position = position;

            return floatingText;
        }
    }
}
