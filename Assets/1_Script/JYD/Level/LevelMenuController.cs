using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Swift_Blade.Level
{
    public class LevelMenuController : MonoBehaviour
    {
        public List<Transform> levels;
        public Transform player;
        
        public int currentLevel = 0;

        [Space] 
        public string baseSceneName;
        public bool useDistanceAdjustment;
        [Range(0.1f , 3)] public float moveTime;
        public float rotateSpeed = 0.5f;
        
        private bool isMoing;

        [SerializeField] private FadeController _fadeController;
        
        private void Start()
        {
            if (_fadeController == null)
                _fadeController = FindObjectsByType<FadeController>(FindObjectsSortMode.None).FirstOrDefault();
            
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
            
            isMoing = true;
            _fadeController.StartFade(()=> SceneManager.LoadScene($"{baseSceneName}_{currentLevel + 1}"),()=> isMoing = false);
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
                        
            float adjustedMoveTime = moveTime * distance; 
        
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