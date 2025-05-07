using Swift_Blade.Inputs;
using UnityEngine;

namespace Swift_Blade.Test
{
    public class TestMinki : MonoBehaviour
    {
        [SerializeField] private PlayerStatCompo _stat;

        [ContextMenu("Stat")]
        private void Stat()
        {
            _stat.AddModifier(StatType.HEALTH, this, 2f);
        }

        [ContextMenu("Test")]
        private void Test()
        {
            InputManager inputManager = InputManager.Instance;
            
            Debug.Log(inputManager.GetCurrentKeyByType(InputType.Movement_Forward));
            Debug.Log(inputManager.GetCurrentKeyByType(InputType.Movement_Left));
            Debug.Log(inputManager.GetCurrentKeyByType(InputType.Roll));
            Debug.Log(inputManager.GetCurrentKeyByType(InputType.Attack2));
        }
    }
}
