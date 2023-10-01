using UnityEngine;

[ExecuteInEditMode]
public class SizeAnimationEditor : Singleton<SizeAnimationEditor>
{
    [Range(.0f, 6.5f)] public float Timer;
    public AnimationCurve Size_curve;
    public float Time;

    void Update()
    {
        transform.localScale = Vector3.one * Size_curve.Evaluate(Timer);
    }
}
