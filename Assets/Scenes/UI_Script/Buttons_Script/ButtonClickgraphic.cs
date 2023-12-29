using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using TMPro;

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

    public TextMeshProUGUI graphic_TXT;

    bool clickedFirst_Graphic = true;

    [SerializeField] private DayNightTest01 day_night;

    private void Start()
    {
        if (SystemInfo.systemMemorySize > 8000)
        {
            graphic_TXT.text = "HIGH";
        }
        else
        {
            graphic_TXT.text = " LOW";
            day_night.time_cycle_activate = false;
        }
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (clickedFirst_Graphic)
        {
            GraphicsSettings.defaultRenderPipeline = defaultRenderPipelineAsset;
            QualitySettings.renderPipeline = defaultRenderPipelineAsset;
            global.profile = volumeLow;
            graphic_TXT.text = " LOW";
            day_night.time_cycle_activate = false;
        }
        else
        {
            GraphicsSettings.defaultRenderPipeline = overrideRenderPipelineAsset;
            QualitySettings.renderPipeline = overrideRenderPipelineAsset;
            global.profile = volumeHigh;
            graphic_TXT.text = "HIGH";
            day_night.time_cycle_activate = true;
        }
        base.OnPointerClick(eventData);
        clickedFirst_Graphic = !clickedFirst_Graphic;
    }
}
