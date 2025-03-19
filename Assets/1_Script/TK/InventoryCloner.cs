using System;
using UnityEngine;

namespace Swift_Blade
{
    public class InventoryCloner : MonoBehaviour
    {
        [SerializeField] private PlayerInventory playerInv;

        private void Start()
        {
            //What.. I can think more....
            InventoryManager.Inventory = playerInv.Clone();
            InventoryManager.Instance.InitializeSlots();
            InventoryManager.IsAfterInit = true;
        }
    }
}
