using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class SerializableKeyValuePair<TKey, TValue>
{
    public TKey key;
    public TValue value;
}

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<SerializableKeyValuePair<TKey, TValue>> _keyValueList =
        new List<SerializableKeyValuePair<TKey, TValue>>();
    
    public void OnBeforeSerialize()
    {
        if (this.Count < _keyValueList.Count) return;
        
        _keyValueList.Clear();

        foreach (var kvp in this)
        {
            _keyValueList.Add(new SerializableKeyValuePair<TKey, TValue>()
            {
                key = kvp.Key,
                value = kvp.Value
            });
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();
        foreach (var kvp in _keyValueList)
        {
            if (this.ContainsKey(kvp.key))
            {
                return;
            }
            
            this.TryAdd(kvp.key, kvp.value);
        }
    }
}

[CustomPropertyDrawer(typeof(SerializableKeyValuePair< , >), true)]
public class SerializableKeyValuePairDrawer : PropertyDrawer
{
    private SerializedProperty _key;
    private SerializedProperty _value;
    private string _name;
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        Rect labelRect = new Rect(
            position.x, position.y, EditorGUIUtility.labelWidth, position.height);
        EditorGUI.LabelField(labelRect, "Key & Value");

        float fieldWidth = (position.width - EditorGUIUtility.labelWidth) / 2 - 2;
        Rect valueRect = new Rect(
            position.x + EditorGUIUtility.labelWidth, position.y, fieldWidth, position.height);
        Rect valueRect2 = new Rect(
            valueRect.x + fieldWidth + 4, position.y, fieldWidth, position.height);
        
        SerializedProperty keyProperty = property.FindPropertyRelative("key");
        SerializedProperty valueProperty = property.FindPropertyRelative("value");
        
        EditorGUI.PropertyField(valueRect, keyProperty, GUIContent.none);
        EditorGUI.PropertyField(valueRect2, valueProperty, GUIContent.none);
        
        EditorGUI.EndProperty();
    }
}
