using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Color_Manager : Singleton<Color_Manager>
{
    [SerializeField] private List<Shader_Color> car_shader_colors;
    public Shader_Color GetRandomShaderColor => car_shader_colors[Random.Range(0, car_shader_colors.Count)];
}
[Serializable]
public struct Shader_Color
{
    public Color top_color;
    public Color bottom_color;
}