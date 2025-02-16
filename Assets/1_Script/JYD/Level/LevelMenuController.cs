using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


namespace Swift_Blade.Level
{
    public class LevelMenuController : MonoBehaviour
    {
        public LevelClearEventSO levelEvent;
        
        public List<Transform> levels;
        public Transform player;
        
        public int currentLevel = 0;

        [Space] 
        public string baseSceneName;
        public bool useDistanceAdjustment;
        [Range(0.1f , 10)] public float moveSpeed;
        public float rotateSpeed = 0.5f;
        
        private bool isMoing;
        
                
        private void Start()
        {
            MovePlayer(currentLevel);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                NextLevel();
            }
            
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PrevLevel();
            }
            
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SelectLevel();
            }
            
        }

        private void SelectLevel()
        {
            if (isMoing) return;
            
            string currentLevelStr = $"{baseSceneName}_{currentLevel + 1}";
                        
            levelEvent.SceneMoveEvent?.Invoke(currentLevelStr , ()=> isMoing = false);
            isMoing = true;
        }

        private void NextLevel()
        {
            if (levels.Count - 1 <= currentLevel || isMoing) return;
            MovePlayer(++currentLevel);
        }
        
        private void PrevLevel()
        {
            if (currentLevel <= 0 || isMoing) return;
            MovePlayer(--currentLevel);
        }

        private void MovePlayer(int levelIndex)
        {
            isMoing = true;
            
            Vector3 targetPos = levels[levelIndex].position;
            Vector3 direction = (targetPos - player.position).normalized;

            float distance = useDistanceAdjustment ? Vector3.Distance(player.position, targetPos) : 1;

            float adjustedMoveTime = distance / moveSpeed;  
            
            if (direction != Vector3.zero)
            {
                float targetYRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                player.transform.DORotate(new Vector3(0, targetYRotation, 0), rotateSpeed);
            }

            player.transform.DOMove(targetPos, adjustedMoveTime).OnComplete(() =>
            {
                isMoing = false;
            });
        }


    }
}