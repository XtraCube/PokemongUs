using System.Reflection;
using Cpp2IL.Core.Extensions;
using Il2CppInterop.Runtime;
using UnityEngine;

namespace PokeLobby;

public static class PokeResources
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public static AssetBundle Bundle { get; private set; }

    public static GameObject PokeCenterPrefab { get; private set; }
    public static GameObject PokeballMachinePrefab { get; private set; }

    public static AudioClip SlideSound { get; private set; }
    public static AudioClip HoverSound { get; private set; }
    public static AudioClip ClickSound { get; private set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public static void Load()
    {
        var bundleStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PokemonLobby.Resources.lobby")
            ?? throw new System.Exception("Failed to load embedded asset bundle: lobby");

        var data = bundleStream.ReadBytes();

        Bundle = AssetBundle.LoadFromMemory(data)
            ?? throw new System.Exception("Failed to load asset bundle from memory: lobby");

        PokeCenterPrefab = Bundle.LoadAsset("PokeCenter", Il2CppType.Of<GameObject>()).Cast<GameObject>();
        PokeballMachinePrefab = Bundle.LoadAsset("PokeballMenu", Il2CppType.Of<GameObject>()).Cast<GameObject>();
        SlideSound = Bundle.LoadAsset("Slide", Il2CppType.Of<AudioClip>()).Cast<AudioClip>();
        HoverSound = Bundle.LoadAsset("Hover", Il2CppType.Of<AudioClip>()).Cast<AudioClip>();
        ClickSound = Bundle.LoadAsset("Collect", Il2CppType.Of<AudioClip>()).Cast<AudioClip>();

        PokeCenterPrefab.DontDestroy();
        PokeballMachinePrefab.DontDestroy();
        SlideSound.DontDestroy();
        HoverSound.DontDestroy();
        ClickSound.DontDestroy();
    }

    private static void DontDestroy(this Object @object)
    {
        @object.hideFlags |= HideFlags.HideAndDontSave;
        Object.DontDestroyOnLoad(@object);
    }
}
