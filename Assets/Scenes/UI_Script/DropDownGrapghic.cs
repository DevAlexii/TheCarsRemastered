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

    private void Start()
    {
        StartGraphicSet();
    }

    private void StartGraphicSet()
    {
        if (SystemInfo.graphicsMemorySize > 8000)
        {
            GraphicsSettings.defaultRenderPipeline = overrideRenderPipelineAsset;
            QualitySettings.renderPipeline = overrideRenderPipelineAsset;
            global.profile = volumeHigh;
            Debug.Log("Graphic set to High");
        }
        else
        {
            GraphicsSettings.defaultRenderPipeline = defaultRenderPipelineAsset;
            QualitySettings.renderPipeline = defaultRenderPipelineAsset;
            global.profile = volumeLow;
            Debug.Log("Graphic set to Low");
        }
    }
}
