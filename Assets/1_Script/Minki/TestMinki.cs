using Swift_Blade.Inputs;
using UnityEngine;

namespace Swift_Blade.Test
{
    public class TestMinki : MonoBehaviour
    {
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
