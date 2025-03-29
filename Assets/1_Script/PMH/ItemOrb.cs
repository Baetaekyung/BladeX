using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Swift_Blade
{
    public class ItemOrb : MonoBehaviour
    {
        [SerializeField] private ItemTableSO itemTables;
        [SerializeField] private ItemDataSO ItemData;

        [SerializeField] private float size;
        [SerializeField] private float duration;
        
        [SerializeField] private Material[] colors;
        private MeshRenderer itemRenderer;
        
        private void Start()
        {
            transform.DOScale(size,0.5f);
        }

        private void OnDestroy()
        {
            DOTween.Kill(gameObject);
        }

        private void Collected()
        {
            int n = Random.Range(0, itemTables.itemTable.Count);
            ItemData = itemTables.itemTable[n].itemData;

            InventoryManager.Instance.AddItemToEmptySlot(ItemData);
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                Debug.Log("¿‚æ“¥Ÿ! ≥Õ ≥ª≤®æﬂ!");
                Collected();
                Destroy(gameObject);
            }
        }

        public void SetColor(ColorType color)
        {
            if(itemRenderer == null)
                itemRenderer = GetComponent<MeshRenderer>();
            
            itemRenderer.material = colors[(int)color];
        }
        
    }
}
