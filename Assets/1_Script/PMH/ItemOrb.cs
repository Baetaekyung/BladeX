using UnityEngine;

namespace Swift_Blade
{
    public class ItemOrb : MonoBehaviour
    {
        public ItemDataSO ItemData { get; set; }

        private void Collected()
        {
            InventoryManager.Instance.AddItemToEmptySlot(ItemData);
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                Debug.Log("¿‚æ“¥Ÿ! ≥Õ ≥ª≤®æﬂ!");
                Collected();
                Destroy(gameObject);
            }
        }
    }
}
