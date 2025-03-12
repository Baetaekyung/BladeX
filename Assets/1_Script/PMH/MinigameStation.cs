using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Swift_Blade
{
    public class MinigameStation : MonoBehaviour, IInteractable
    {
        [SerializeField] private string gameSceneName;
        [SerializeField] private LevelClearEventSO clearSO;

        public void Interact()
        {
            Debug.Log("Interacted");
            clearSO.SceneChangeEvent?.Invoke(gameSceneName);
        }
    }
}
