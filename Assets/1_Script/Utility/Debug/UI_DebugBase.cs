using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
public static class DBG_UI_KEYS
{
    #region Keys
    //starts with 1 because default(int) is 0 and DebugText's key parameter is zero
    public const int Keys_PlayerMovement = 1;
    public const int Keys_PlayerAction = 2;
    public const int Keys_2 = 3;
    public const int Keys_3 = 4;
    #endregion
}

public abstract class UI_DebugBase<T> : MonoSingleton<T> where T : UI_DebugBase<T>
{
    public abstract int Key { get; set; }
    public bool ShowDebugUI
    {
        get => active;
        set
        {
            active = value;
            OnChange();
        }
    }
    [SerializeField] private bool active = true;
    [SerializeField] private List<TextMeshProUGUI> list;
    private readonly StringBuilder stringBuilder = new StringBuilder(16);
    protected virtual void Start() => OnChange();
    private void OnChange() => gameObject.SetActive(ShowDebugUI);
    private static void SetDebug(in int index, in string str, string prefix = "", in int key = 0)
    {
        T instance = Instance;
        if (key != instance.Key || instance == null) return;
        instance.stringBuilder.Clear();
        instance.stringBuilder.Append(prefix);
        instance.stringBuilder.Append(" : ");
        instance.stringBuilder.Append(str);
        instance.list[index].text = instance.stringBuilder.ToString();
    }
    public static void DebugText(int index, string stringValue, string prefix = "", int key = 0) => SetDebug(index, stringValue, prefix, key);
    public static void DebugText<VT>(int index, VT valueType, string prefix = "", int key = 0)
    {
        SetDebug(index, valueType.ToString(), prefix, key);
    }


}
