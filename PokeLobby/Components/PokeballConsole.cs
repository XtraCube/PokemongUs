using System;
using UnityEngine;

namespace PokeLobby.Components;

public class PokeballConsole(IntPtr ptr) : MonoBehaviour(ptr)
{
    public float UsableDistance => 1.4f;

    public float PercentCool => 0;

    public ImageNames UseIcon => ImageNames.UseButton;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public SpriteRenderer outline;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public void Awake()
    {
        outline = GetComponent<SpriteRenderer>();
        outline.material = new Material(Shader.Find("Sprites/Outline"));
    }

    public float CanUse(NetworkedPlayerInfo pc, out bool canUse, out bool couldUse)
    { 
        var num = float.MaxValue;
        var @object = pc.Object;
        couldUse = @object.CanMove;
        canUse = couldUse;
        if (!canUse)
        {
            return num;
        }

        num = Vector2.Distance(@object.GetTruePosition(), transform.position);
        canUse &= num <= UsableDistance;
        return num;
    }

    public void SetOutline(bool on, bool mainTarget)
    {
        if (!outline)
        {
            return;
        }

        outline.material.SetFloat(ShaderID.Outline, on ? 1 : 0);
        outline.material.SetColor(ShaderID.OutlineColor, Color.white);
        outline.material.SetColor(ShaderID.AddColor, mainTarget ? Color.white : Color.clear);
    }

    public void Use()
    {
        PlayerControl.LocalPlayer.NetTransform.Halt();
        var pokeballMenu = Instantiate(PokeResources.PokeballMachinePrefab, HudManager.Instance.transform);
        pokeballMenu.GetComponent<PokeballMenu>().Begin(null);
    }
}
