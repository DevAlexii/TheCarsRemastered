using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Color_Manager : Singleton<Color_Manager>
{
    [Header("Car")]
    [SerializeField] private List<Shader_Color> car_shader_colors;
    public Shader_Color GetRandomShaderColor => car_shader_colors[Random.Range(0, car_shader_colors.Count)];

    [Header("Pedestrian")]
    [SerializeField] List<PoolColors> colors;
    public Dictionary<bodyPart, List<Shader_Color>> bodies_colors { get; private set; }
    private void Start()
    {
        bodies_colors = new Dictionary<bodyPart, List<Shader_Color>>();
        for (int i = 0; i < colors.Count; i++)
        {
            bodies_colors[colors[i].key] = colors[i].values;
        }
    }

}
[Serializable]
public struct Shader_Color
{
    public Color top_color;
    public Color bottom_color;
}
[Serializable]
public struct PoolColors
{
    public bodyPart key;
    public List<Shader_Color> values;
}