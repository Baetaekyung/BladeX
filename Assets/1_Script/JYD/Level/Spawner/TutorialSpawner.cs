using System.Collections;
using Swift_Blade.Level;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade
{
    public class TutorialSpawner : Spawner,IInteractable
    {
        [SerializeField] private DialogueDataSO dialogueData;

        protected override void Start()
        {
            InitializeParticle();
        }
                
        public void Interact()
        {
            DialogueManager.Instance.StartDialogue(dialogueData).Subscribe(HandleDialogueEndEvent);
        }
        
        private void HandleDialogueEndEvent()
        {
            StartCoroutine(Spawn());
        }
        
        protected override IEnumerator Spawn()
        {
            for (int i = 0; i < spawnEnemies.Count; i++)
            {
                for (int j = 0; j < spawnEnemies[i].spawnInfos.Length; j++)
                {
                    MonoGenericPool<SmallSpawnParticle>.Pop().transform.position =
                        spawnEnemies[i].spawnInfos[j].spawnPosition.position;
                    
                    Instantiate(spawnEnemies[i].spawnInfos[j].enemy, spawnEnemies[i].spawnInfos[j].spawnPosition.position, Quaternion.identity);
                }   
            }       
            yield break;
        }

        
    }
}
