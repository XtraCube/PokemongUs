using System.Linq;
using System.Reflection;
using Cpp2IL.Core.Extensions;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
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

    public static Sprite[] PikachuSprites { get; private set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public static void Load()
    {
        Bundle = AssetBundleManager.Load("lobby");
        if (Bundle == null)
        {
            throw new System.Exception("Failed to load asset bundle: lobby");
        }

        PokeCenterPrefab = Bundle.LoadAsset<GameObject>("PokeCenter")!;
        PokeballMachinePrefab = Bundle.LoadAsset<GameObject>("PokeballMenu")!;
        SlideSound = Bundle.LoadAsset<AudioClip>("Slide")!;
        HoverSound = Bundle.LoadAsset<AudioClip>("Hover")!;
        ClickSound = Bundle.LoadAsset<AudioClip>("Collect")!;
        PikachuSprites = Bundle.LoadAssetWithSubAssets("Pikachu", Il2CppType.Of<Sprite>()).Select(x => x.Cast<Sprite>()).ToArray();
        
        if (PikachuSprites.Length == 0)
        {
            throw new System.Exception("Failed to load Pikachu sprites from asset bundle: lobby");
        }

        PokeCenterPrefab.DontDestroy();
        PokeballMachinePrefab.DontDestroy();
        SlideSound.DontDestroy();
        HoverSound.DontDestroy();
        ClickSound.DontDestroy();
        foreach (var sprite in PikachuSprites)
        {
            sprite.DontDestroy();
        }
    }
}
