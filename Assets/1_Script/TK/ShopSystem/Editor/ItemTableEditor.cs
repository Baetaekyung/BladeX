using UnityEditor;
using UnityEngine;

namespace Swift_Blade
{
    [CustomEditor(typeof(ItemTableSO))]
    public class ItemTableEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(GUILayout.Button("Collect Items"))
            {
                (target as ItemTableSO).CollectAssets();
            }
        }
    }
}
