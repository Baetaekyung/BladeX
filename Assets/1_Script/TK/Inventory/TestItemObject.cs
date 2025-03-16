using UnityEngine;

namespace Swift_Blade
{
    public class TestItemObject : ItemObject
    {
        public override void ItemEffect(Player player)
        {
            Debug.Log("Item을 사용 하였다.");
        }
    }
}
