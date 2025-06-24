using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Unity.IL2CPP.Utils.Collections;
using Il2CppInterop.Runtime.Attributes;
using UnityEngine;

namespace PokeLobby.Components;

public class PikachuBehaviour : MonoBehaviour
{
    public List<Sprite> sprites = [];

    public SpriteRenderer renderer;

    public void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        sprites = PokeResources.PikachuSprites.ToList();
        renderer.sprite = sprites[0];
        StartCoroutine(CoAnimate().WrapToIl2Cpp());
    }

    [HideFromIl2Cpp]
    public IEnumerator CoAnimate()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            renderer.sprite = sprites[Random.Range(0, sprites.Count)];
        }
    }
}