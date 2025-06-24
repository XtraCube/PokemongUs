using System;
using UnityEngine;

namespace PokeLobby.Components;

public class PokeballBehaviour(IntPtr ptr) : MonoBehaviour(ptr)
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public AudioSource sound;
    public SpriteRenderer renderer;
    public CircleCollider2D collider2D;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public bool canCollect;
    public bool isMouseOver;
    public float readyTimestamp;

    private readonly Color[] _colors = [Color.white, Color.yellow];

    private float _time;
        
    public void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<CircleCollider2D>();
        sound = gameObject.AddComponent<AudioSource>();

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

        _time += Time.deltaTime;
        var color = Color.Lerp(_colors[0], _colors[1], Mathf.Abs(Mathf.Sin(_time*3)));
        renderer.material.SetColor(ShaderID.AddColor, !isMouseOver ? color : Color.cyan);
    }

    public void OnMouseEnter()
    {
        if (!canCollect)
        {
            return;
        }

        sound.PlayOneShot(PokeResources.HoverSound);
    }

    public void OnMouseDown()
    {
        if (!canCollect)
        {
            return;
        }

        sound.PlayOneShot(PokeResources.ClickSound);
        renderer.material.SetFloat(ShaderID.Outline, 1f);
        renderer.material.SetColor(ShaderID.OutlineColor, Color.white);
    }
        
    public void OnMouseUp()
    {
        if (!canCollect)
        {
            return;
        }

        renderer.enabled = false;
        collider2D.enabled = false;
    }

    public void OnMouseOver()
    {
        isMouseOver = true;
        if (!canCollect)
        {
            return;
        }

        renderer.material.SetFloat(ShaderID.Outline, 1f);

        if (!Input.GetMouseButton(0))
        {
            renderer.material.SetColor(ShaderID.OutlineColor, Color.yellow);
        }
    }
    public void OnMouseExit()
    {
        isMouseOver = false;
        if (!canCollect)
        {
            return;
        }

        renderer.material.SetFloat(ShaderID.Outline, 0f);
        renderer.material.SetColor(ShaderID.AddColor, Color.clear);
    }
}
