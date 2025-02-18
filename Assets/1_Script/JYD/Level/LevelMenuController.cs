using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;


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
        
        [SerializeField] private bool isMoving;
        
        private void Awake()
        {
            MovePlayer(currentLevel);
        }
        
        private void Update()
        {
            if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            {
                NextLevel();
            }
            
            if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            {
                PrevLevel();
            }
            
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                SelectLevel();
            }
            
        }

        private void SelectLevel()
        {
            if (isMoving) return;
            
            string currentLevelStr = $"{baseSceneName}_{currentLevel + 1}";
                        
            levelEvent.SceneMoveEvent?.Invoke(currentLevelStr , ()=> isMoving = false);
            isMoving = true;
        }

        private void NextLevel()
        {
            if (levels.Count - 1 <= currentLevel || isMoving) return;
            MovePlayer(++currentLevel);
        }
        
        private void PrevLevel()
        {
            if (currentLevel <= 0 || isMoving) return;
            MovePlayer(--currentLevel);
        }

        private void MovePlayer(int levelIndex)
        {
            isMoving = true;
            
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
                isMoving = false;
            });
        }


    }
}