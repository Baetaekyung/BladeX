using Swift_Blade.Combat.Health;
using UnityEngine;

namespace Swift_Blade
{
    public class Menu : MonoBehaviour
    {
        public static bool IsNewGame = true;

        void Start()
        {
            InitLobby();
        } 

        private void InitLobby()
        {
            IsNewGame = false;
            Player.Instance.GetEntityComponent<PlayerHealth>().TakeHeal(999);
        }
    }
}
