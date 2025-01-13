using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "PlayerInventory", menuName = "SO/Shop/PlayerInventory")]
    public class PlayerInventory : ScriptableObject
    {
        public int currentMoney; //������ �� �𸣰���;;

        //public List<ShopItemData> 
        
        public void AddMoney(int price)
        {
            currentMoney += price;
        }

        public void AddItem()
        {
            
        }
    }
}
