using System;
using UnityEngine;

namespace Swift_Blade
{
    public class DefaultBreakableListener : MonoBehaviour
    {
        [SerializeField] private BreakableObject breakableObject;
        [SerializeField] private GameObject brokenModel;
        private void Awake()
        {
            breakableObject.OnDeadStart += OnDeadStart;
            breakableObject.OnGameObjectDestroy += OnGameObjectDestroy;
        }
        private void OnDeadStart(BreakableObject breakableObject)
        {
            brokenModel.SetActive(true);
            breakableObject.gameObject.SetActive(false);
        }
        private void OnGameObjectDestroy(BreakableObject breakableObject)
        {
            Destroy(gameObject);
        }
    }
}
