using System;
using System.Collections.Generic;
using System.Linq;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace PokeLobby.Components;

[RegisterInIl2Cpp]
public class PokeballMenu(IntPtr ptr) : Minigame(ptr)
{
    public List<PokeballBehaviour> pokeballs = [];

    public BoxCollider2D clickToClose;

    public BoxCollider2D background;

    public void Awake()
    {
        clickToClose = GetComponent<BoxCollider2D>();
        background = transform.Find("Background").GetComponent<BoxCollider2D>();
        OpenSound = PokeResources.SlideSound;
        CloseSound = PokeResources.SlideSound;
        logger = new Logger(Logger.Category.Gameplay, "PokeballMenu");
        transform.Find("PokeballGroup").gameObject.AddComponent<AudioSource>();
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
            StartCoroutine(CoStartClose(0));
        }

        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Vector2 inputPosition = Input.mousePosition;
            if (Input.touchCount > 0)
            {
                inputPosition = Input.GetTouch(0).position;
            }

            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(inputPosition);

            if (!background.OverlapPoint(worldPoint) && clickToClose.OverlapPoint(worldPoint))
            {
                StartCoroutine(CoStartClose(0));
            }
        }

        if (!pokeballs.Any(x => x.gameObject.active))
        {
            StartCoroutine(CoStartClose(.5f));
        }
    }
}
