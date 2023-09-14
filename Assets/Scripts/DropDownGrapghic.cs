using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Rendering;

public class DropDownGrapghic : MonoBehaviour
{
    [SerializeField] RenderPipelineAsset assetsLow;
    [SerializeField] RenderPipelineAsset assetsHigh;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void HandleInputData(int var)
    {
        if (var == 0)
        {
            GraphicsSettings.defaultRenderPipeline = assetsLow;
        }
        if (var == 1)
        {
            GraphicsSettings.defaultRenderPipeline = assetsHigh;

        }
    }
}
