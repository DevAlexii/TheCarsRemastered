using UnityEngine;

public class RampPrefabFunciont : MonoBehaviour
{
    private bool bounceFinish;
    private float bounceTimer;
    [SerializeField] private float MaxbounceTime;
    [SerializeField] private AnimationCurve bounceCurve;
    private float lifetime;
    [SerializeField] private GameObject effect;
    private float timer;
    private AnimationCurve size_curve;


    void Update()
    {
        if (!bounceFinish)
        {
            bounceTimer += Time.deltaTime;
            if (bounceTimer >= MaxbounceTime)
            {
                bounceFinish = true;
                Camera.main.GetComponent<CameraShake>().StartShake(0.50f);
                effect.SetActive(true);
                lifetime = SizeAnimationEditor.self.Time;
                size_curve = SizeAnimationEditor.self.Size_curve;
                Destroy(effect, 1f);
                Destroy(this.gameObject, lifetime);
            }
            float y = bounceCurve.Evaluate(bounceTimer);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);


        }

        if (lifetime != 0)
        {
            timer += Time.deltaTime;
            if (timer > 1)
            {
                transform.GetChild(0).transform.localScale = Vector3.one * size_curve.Evaluate(timer);
            }
           
        }
    }
}
