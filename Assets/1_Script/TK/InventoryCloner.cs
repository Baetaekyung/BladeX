using System;
using UnityEngine;

namespace Swift_Blade
{
    public class InventoryCloner : MonoBehaviour
    {
        [SerializeField] private PlayerInventory playerInv;

        private void Start()
        {
            //이게 맞나..   
            InventoryManager.Inventory = playerInv.Clone();
            InventoryManager.Instance.InitializeSlots();
            InventoryManager.IsAfterInit = true;
        }
    }
}
