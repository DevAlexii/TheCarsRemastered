using UnityEngine;

public class PowerUpBase : MonoBehaviour, I_Interface
{
    [SerializeField] protected GameObject effect;
    private float time;
    private float timer;
    private AnimationCurve size_curve;

    private void Start()
    {
        time = SizeAnimationEditor.self.Time;
        size_curve = SizeAnimationEditor.self.Size_curve;
        Destroy(this.gameObject, 3f);
    }
    public virtual void OnClicked()
    {
        AudioCallBack.self.PlayAudio(AudioType.Coin,0.8f);
        Destroy(this.gameObject);
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > time) { Destroy(this.gameObject); }
        transform.localScale = Vector3.one * size_curve.Evaluate(timer);
    }
}
