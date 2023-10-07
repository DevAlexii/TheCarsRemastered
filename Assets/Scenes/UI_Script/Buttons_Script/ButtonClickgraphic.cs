using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class ButtonClickgraphic : ButtonClickParent
{
    //Function to Chnage Graphic
    [Header("RenderPipelineRef")]
    [SerializeField] RenderPipelineAsset defaultRenderPipelineAsset;
    [SerializeField] RenderPipelineAsset overrideRenderPipelineAsset;

    [Header("VolumesProfilersRef")]
    [SerializeField] VolumeProfile volumeHigh;
    [SerializeField] VolumeProfile volumeLow;

    [Header("VolumeInSceneRef")]
    [SerializeField] Volume global;

    bool clickedFirst_Graphic = true;

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (clickedFirst_Graphic)
        {
            GraphicsSettings.defaultRenderPipeline = defaultRenderPipelineAsset;
            QualitySettings.renderPipeline = defaultRenderPipelineAsset;
            global.profile = volumeLow;
            transform.GetComponent<Image>().sprite = newImage;

        }
        else
        {
            GraphicsSettings.defaultRenderPipeline = overrideRenderPipelineAsset;
            QualitySettings.renderPipeline = overrideRenderPipelineAsset;
            global.profile = volumeHigh;
            transform.GetComponent<Image>().sprite = baseImage;
        }
        base.OnPointerClick(eventData);
        clickedFirst_Graphic = !clickedFirst_Graphic;
    }
}
