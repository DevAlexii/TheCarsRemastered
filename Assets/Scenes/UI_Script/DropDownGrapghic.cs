using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;

public class DropDownGrapghic : MonoBehaviour
{
    [Header("RenderPipelineRef")]
    [SerializeField] RenderPipelineAsset defaultRenderPipelineAsset;
    [SerializeField] RenderPipelineAsset overrideRenderPipelineAsset;

    [Header("VolumesProfilersRef")]
    [SerializeField] VolumeProfile volumeHigh;
    [SerializeField] VolumeProfile volumeLow;

    [Header("VolumeInSceneRef")]
    [SerializeField] Volume global;

    [Header("LightSettings")]
    [SerializeField] LightingSettings lightSettingsLow;
    [SerializeField] LightingSettings lightSettingsHigh;


    public void HandleInputData(int var)
    {
        if (var == 0)
        {
            GraphicsSettings.defaultRenderPipeline = defaultRenderPipelineAsset;
            QualitySettings.renderPipeline = defaultRenderPipelineAsset;

            global.profile = volumeLow;

            Lightmapping.lightingSettings = lightSettingsLow;
        }
        if (var == 1)
        {
            GraphicsSettings.defaultRenderPipeline = overrideRenderPipelineAsset;
            QualitySettings.renderPipeline = overrideRenderPipelineAsset;
            global.profile = volumeHigh;

            Lightmapping.lightingSettings = lightSettingsHigh;
        }
    }
}
