using UnityEngine;

namespace PokeLobby;

public static class ShaderID
{
    public static int Outline { get; } = Shader.PropertyToID("_Outline");
    public static int OutlineColor { get; } = Shader.PropertyToID("_OutlineColor");
    public static int AddColor { get; } = Shader.PropertyToID("_AddColor");
}