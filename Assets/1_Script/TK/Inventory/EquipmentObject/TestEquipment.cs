using System.Runtime.CompilerServices;
using UnityEngine;

namespace Swift_Blade
{
    public class TestEquipment : BaseEquipment
    {
        public override void ItemEffect()
        {
            Debug.Log("Item ȿ�� �ߵ�");
        }

        public override void Interact()
        {
            Debug.Log("������ ȹ��");
        }
    }
}
