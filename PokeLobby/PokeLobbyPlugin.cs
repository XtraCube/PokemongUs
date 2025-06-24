using System.Linq;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using PokeLobby.Components;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PokeLobby;

[BepInAutoPlugin("dev.xtracube.pokelobby", "Pokemon Lobby Mod")]
[BepInProcess("Among Us.exe")]
public partial class PokeLobbyPlugin : BasePlugin
{
    private Harmony Harmony { get; } = new(Id);
    private static Vector2[] SpawnPositions { get; } = Enumerable.Repeat(new Vector2(-3.1f, 3.3f), 5).Concat(Enumerable.Repeat(new Vector2(3.3f, 2f), 5)).ToArray();

    public override void Load()
    {
        ClassInjector.RegisterTypeInIl2Cpp<PokeballBehaviour>();
        ClassInjector.RegisterTypeInIl2Cpp<PokeballMenu>();
        ClassInjector.RegisterTypeInIl2Cpp<PokeballConsole>(new RegisterTypeOptions
        {
            Interfaces = new Il2CppInterfaceCollection([typeof(IUsable)])
        });
        PokeResources.Load();
        Harmony.PatchAll();
    }

    [HarmonyPatch(typeof(LobbyBehaviour), nameof(LobbyBehaviour.Start))]
    public static class LobbyBehaviourStartPatch
    {
        public static void Postfix(LobbyBehaviour __instance)
        {
            Object.Instantiate(PokeResources.PokeCenterPrefab, __instance.transform);
            ModifyLobby(__instance);
        }
    }

    public static void ModifyLobby(LobbyBehaviour lobby)
    {
        // set lobby prefab
        lobby.SpawnPositions = SpawnPositions;

        // remove original lobby background, collider, and boxes
        Object.DestroyImmediate(lobby.transform.Find("Background").gameObject);
        Object.DestroyImmediate(lobby.transform.Find("RightBox").gameObject);
        Object.DestroyImmediate(lobby.transform.Find("Leftbox").gameObject);
        Object.DestroyImmediate(lobby.GetComponent<EdgeCollider2D>());

        // move engines to correct positions
        var rEngine = lobby.transform.Find("RightEngine");
        rEngine.position = new Vector3(5.1f, 1.5f, 10.5f);
        rEngine.localScale = new Vector3(0.8f, 0.8f, 1f);
            
        var lEngine = lobby.transform.Find("LeftEngine");
        lEngine.position = new Vector3(-5.1f, 1.5f, 10.5f);
        lEngine.localScale = new Vector3(0.8f, 0.8f, 1f);

        // adjust small box position and remove collider
        var smallBox = lobby.transform.Find("SmallBox");

        // destroy options console box and collider
        Object.DestroyImmediate(smallBox.GetComponent<SpriteRenderer>());
        Object.DestroyImmediate(smallBox.GetComponent<PolygonCollider2D>());

        smallBox.localPosition = new Vector3(0.9f, 2.9f, -9.9f);
        smallBox.GetComponentInChildren<SpriteRenderer>().flipX = true;

        // move wardrobe console to table
        lobby.transform.Find("panel_Wardrobe").localPosition = new Vector3(2.13f, 1.57f, -9.9f);
    }
}
