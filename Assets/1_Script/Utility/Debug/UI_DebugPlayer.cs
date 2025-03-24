using UnityEngine;

public class UI_DebugPlayer : UI_DebugBase<UI_DebugPlayer>
{
    public override int Key { get; set; } = DBG_UI_KEYS.Keys_PlayerMovement;
}