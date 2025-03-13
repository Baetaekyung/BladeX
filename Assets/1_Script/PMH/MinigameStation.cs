using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Swift_Blade
{
    public class MinigameStation : MonoBehaviour, IInteractable
    {
        [SerializeField] private string gameSceneName;
        [SerializeField] private SceneManagerSO clearSO;

        public void Interact()
        {
            Debug.Log("Interacted");
            clearSO.LoadScene(gameSceneName);
        }
    }
}
