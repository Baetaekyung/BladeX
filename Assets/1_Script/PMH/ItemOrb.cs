using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Swift_Blade
{
    public class ItemOrb : MonoBehaviour,IInteractable
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

        public void SetColor(ColorType color)
        {
            if(itemRenderer == null)
                itemRenderer = GetComponent<MeshRenderer>();
            
            itemRenderer.material = colors[(int)color];
        }

        public void Interact()
        {
            DOTween.Kill(gameObject);
            transform.DOScale(0 , duration).SetEase(Ease.OutSine).OnComplete(() =>
            {
                int n = Random.Range(0, itemTables.itemTable.Count);
                ItemData = itemTables.itemTable[n].itemData;
            
                InventoryManager.Instance.AddItemToEmptySlot(ItemData);
            });
            
        }
    }
}
