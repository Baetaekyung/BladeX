using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
public static class UI_DB_KEYS
{
    #region Keys
    //starts with 1 because default(int) is 0 and DebugText's key parameter is zero
    public const int Keys_PlayerMovement    = 1;
    public const int Keys_PlayerAction      = 2;
    public const int Keys_2                 = 3;
    public const int Keys_3                 = 4;
    #endregion
}

public abstract class UI_DebugBase<T> : MonoSingleton<T> where T : UI_DebugBase<T>
{
    public static int Key { get; set; } = UI_DB_KEYS.Keys_PlayerMovement;
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
    private readonly StringBuilder stringBuilder = new();
    protected virtual void Start()
    {
        OnChange();
    }
    private void OnChange()
    {
        gameObject.SetActive(ShowDebugUI);
    }
    public void DebugText(int index, string stringValue, string prefix = "", int key = 0)
    {
        if (key != Key) return;
        stringBuilder.Clear();
        stringBuilder.Append(prefix);
        stringBuilder.Append(" : ");
        stringBuilder.Append(stringValue);
        list[index].text = stringBuilder.ToString();
    }
    public void DebugText(int index, ValueType valueType, string prefix = "", int key = 0)
    {
        if (key != Key) return;
        stringBuilder.Clear();
        stringBuilder.Append(prefix);
        stringBuilder.Append(" : ");
        stringBuilder.Append(valueType.ToString());
        list[index].text = stringBuilder.ToString();
    }

}
