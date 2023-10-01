using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration; 
    public float shakeMagnitude = 0.1f; 

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
        shakeDuration = 0;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = originalPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = originalPosition;
        }
    }

    public void StartShake()
    {
        if (shakeDuration <= 0)
        {
            originalPosition = transform.localPosition;
            shakeDuration = 0.5f;
        }
    }
}