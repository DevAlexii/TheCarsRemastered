using UnityEngine;

public class RampPrefabFunciont : MonoBehaviour
{
    private bool bounceFinish;
    private float bounceTimer;
    [SerializeField] private float MaxbounceTime;
    [SerializeField] private AnimationCurve bounceCurve;
    [SerializeField] private float lifetime;
    [SerializeField] private GameObject effect;

    void Update()
    {
        if (!bounceFinish)
        {
            bounceTimer += Time.deltaTime;
            if (bounceTimer >= MaxbounceTime)
            {
                bounceFinish = true;
                Camera.main.GetComponent<CameraShake>().StartShake(0.20f);
                Instantiate(effect, transform.position, Quaternion.identity);
                Destroy(effect, 1f);
                Destroy(this.gameObject, lifetime);
            }
            float y = bounceCurve.Evaluate(bounceTimer);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }
    }
}