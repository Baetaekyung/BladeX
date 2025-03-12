using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Swift_Blade.Level
{

    [System.Serializable]
    public enum MoveDirection
    {
        Up,
        Right,
        Down,
        Left
    }
    
    public class LevelMenuController : MonoBehaviour
    {
        public LevelClearEventSO levelEvent;
        
        public List<Transform> levels;
        public MoveDirection[] NextStageDirections;
        public MoveDirection[] PreviousStageDirections;
        
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
            if (isMoving) return;
            
            PrevLevels();
            NextLevels();
            SelectLevel();
        }

        private void PrevLevels()
        {
            if (currentLevel > 0)  
            {
                switch (PreviousStageDirections[currentLevel - 1])
                {
                    case MoveDirection.Up when Keyboard.current.upArrowKey.wasPressedThisFrame:
                    case MoveDirection.Right when Keyboard.current.rightArrowKey.wasPressedThisFrame:
                    case MoveDirection.Down when Keyboard.current.downArrowKey.wasPressedThisFrame:
                    case MoveDirection.Left when Keyboard.current.leftArrowKey.wasPressedThisFrame:
                        PrevLevel();
                        break;
                }
            }
                        
        }

        private void NextLevels()
        {
            if (currentLevel >= levels.Count - 1) 
                return;
    
            switch (NextStageDirections[currentLevel])
            {
                case MoveDirection.Up when Keyboard.current.upArrowKey.wasPressedThisFrame:
                case MoveDirection.Right when Keyboard.current.rightArrowKey.wasPressedThisFrame:
                case MoveDirection.Down when Keyboard.current.downArrowKey.wasPressedThisFrame:
                case MoveDirection.Left when Keyboard.current.leftArrowKey.wasPressedThisFrame:
                    NextLevel();
                    break;
            }
        }

        
        private void SelectLevel()
        {
            if (isMoving) return;
            
            if (Keyboard.current.enterKey.wasPressedThisFrame 
                || Mouse.current.leftButton.wasPressedThisFrame
                || Mouse.current.rightButton.wasPressedThisFrame)
            {
                string currentLevelStr = levels[currentLevel].name;
                
                levelEvent.SceneMoveEvent?.Invoke(currentLevelStr , ()=> isMoving = false);
                isMoving = true;
            }
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
