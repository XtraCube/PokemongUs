using System;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace PokeLobby.Components;

[RegisterInIl2Cpp]
public class PokeballBehaviour(IntPtr ptr) : MonoBehaviour(ptr)
{
    public AudioSource sound;
    public SpriteRenderer renderer;
    public CircleCollider2D collider2D;

    public bool canCollect;
    public bool isMouseOver;
    public float readyTimestamp;

    private readonly Color[] _colors = [Color.white, Color.yellow];

    private float _time;
        
    public void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<CircleCollider2D>();
        sound = transform.parent.GetComponent<AudioSource>();

        renderer.material.shader = Shader.Find("Sprites/Outline");
    }

    public void Start()
    {
        readyTimestamp = Time.time + UnityEngine.Random.Range(1f, 8f);
    }
        
    public void Update()
    {
        canCollect = readyTimestamp <= Time.time;
        if (!canCollect)
        {
            return;
        }

        var inputPosition = Input.mousePosition;
        if (Input.touchCount > 0)
        {
            inputPosition = Input.GetTouch(0).position;
        }
        var worldPoint = Camera.main.ScreenToWorldPoint(inputPosition);
        if (collider2D.OverlapPoint(worldPoint))
        {
            if (!isMouseOver)
            {
                isMouseOver = true;
                sound.PlayOneShot(PokeResources.HoverSound);
            }

            renderer.material.SetColor(ShaderID.OutlineColor, Color.yellow);

            if (Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved))
            {
                sound.PlayOneShot(PokeResources.ClickSound);
                gameObject.SetActive(false);
            }
        }
        else
        {
            isMouseOver = false;
        }

        renderer.material.SetFloat(ShaderID.Outline, isMouseOver ? 1f : 0f);

        _time += Time.deltaTime;
        var color = Color.Lerp(_colors[0], _colors[1], Mathf.Abs(Mathf.Sin(_time*3)));
        renderer.material.SetColor(ShaderID.AddColor, isMouseOver ? Color.cyan : color);
    }
}
