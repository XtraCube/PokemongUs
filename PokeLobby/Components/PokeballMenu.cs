using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PokeLobby.Components;

public class PokeballMenu(IntPtr ptr) : Minigame(ptr)
{
    public List<PokeballBehaviour> pokeballs = [];

    public void Awake()
    {
        OpenSound = PokeResources.SlideSound;
        CloseSound = PokeResources.SlideSound;
        logger = new Logger(Logger.Category.Gameplay, "PokeballMenu");
    }

    public void Start()
    {
        pokeballs.AddRange(GetComponentsInChildren<PokeballBehaviour>());
    }

    public void OnDestroy()
    {
        PlayerControl.LocalPlayer.moveable = true;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(CoStartClose());
        }

        if (!pokeballs.Any(x => x.renderer.enabled))
        {
            StartCoroutine(CoStartClose());
        }
    }
}
