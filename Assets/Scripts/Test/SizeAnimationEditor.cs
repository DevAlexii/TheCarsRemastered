using UnityEngine;

[ExecuteInEditMode]
public class SizeAnimationEditor : Singleton<SizeAnimationEditor>
{
    [Range(0, 20)] public float Timer;
    public AnimationCurve Size_curve;
    public float Time;

    [ExecuteAlways]
    private void Start()
    {
        Destroy(GetComponent<MeshFilter>());
        Destroy(GetComponent<MeshRenderer>());
    }


    void Update()
    {
        transform.localScale = Vector3.one * Size_curve.Evaluate(Timer);
    }
}
