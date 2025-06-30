using System.Linq;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using PokeLobby.Components;
using Reactor;
using Reactor.Networking;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PokeLobby;

[BepInAutoPlugin("dev.xtracube.pokelobby", "Pokemon Lobby Mod")]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
[ReactorModFlags(ModFlags.RequireOnAllClients)]
public partial class PokeLobbyPlugin : BasePlugin
{
    private Harmony Harmony { get; } = new(Id);
    private static Vector2[] SpawnPositions { get; } = 
        Enumerable.Repeat(new Vector2(-3.1f, 3.3f), 2)
            .Concat(Enumerable.Repeat(new Vector2(-3.1f, 2.8f), 3))
            .Concat(Enumerable.Repeat(new Vector2(1.9f, 2.35f), 3))
            .Concat(Enumerable.Repeat(new Vector2(2.4f, 2.35f), 3))
            .ToArray();

    public override void Load()
    {
        ReactorCredits.Register("PokeLobby - XtraCube", Version.Split("+")[0], false, ReactorCredits.AlwaysShow);
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
        lobby.transform.Find("Background").gameObject.DestroyImmediate();
        lobby.transform.Find("RightBox").gameObject.DestroyImmediate();
        lobby.transform.Find("Leftbox").gameObject.DestroyImmediate();
        lobby.GetComponent<EdgeCollider2D>().DestroyImmediate();

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
        smallBox.GetComponent<SpriteRenderer>().DestroyImmediate();
        smallBox.GetComponent<PolygonCollider2D>().DestroyImmediate();

        smallBox.localPosition = new Vector3(0.9f, 2.9f, -9.9f);
        smallBox.GetComponentInChildren<SpriteRenderer>().flipX = true;

        // move wardrobe console to table
        lobby.transform.Find("panel_Wardrobe").localPosition = new Vector3(2.13f, 1.57f, -9.9f);
    }
}
