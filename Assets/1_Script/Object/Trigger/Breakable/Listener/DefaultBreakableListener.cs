using UnityEngine;

namespace Swift_Blade
{
    public class DefaultBreakableListener : ListenerBase
    {
        [SerializeField] private BreakableObject breakableObject;
        [SerializeField] private GameObject brokenModel;
        //[SerializeField] private AudioEmitter audioEmitter;
        private void Awake()
        {
            breakableObject.OnDeadStart += OnDeadStart;
            breakableObject.OnGameObjectDestroy += OnGameObjectDestroy;
        }
        private void OnDeadStart()
        {
            brokenModel.SetActive(true);
            FireBaseAction();
            //if(audioEmitter != null)
            //    audioEmitter.Play();
        }
        private void OnGameObjectDestroy()
        {
            Destroy(gameObject);
        }
    }
}
