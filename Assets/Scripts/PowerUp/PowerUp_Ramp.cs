using System.Collections;
using UnityEngine;

public class PowerUp_Ramp : PowerUpBase
{
    [SerializeField] private GameObject ramp_prefab;
    public AnimationCurve bounceCurve;
    private float bounceTime = 1f;
    public override void OnClicked()
    {
        int random_angle = Random.Range(0, 4) * 90;
        Quaternion rot = Quaternion.Euler(Vector3.up * random_angle);
        GameObject ramp = Instantiate(ramp_prefab, Vector3.zero, rot);
        Destroy(ramp,5);

        StartCoroutine(BounceEffect(ramp));
        base.OnClicked();
    }

    private IEnumerator BounceEffect(GameObject ramp)
    {
        float time = 0f;
        Vector3 initialPosition = ramp.transform.position;

        while (time < bounceTime)
        {
            float yOffset = bounceCurve.Evaluate(time / bounceTime);
            ramp.transform.position = initialPosition + new Vector3(0, yOffset, 0);

            time += Time.deltaTime; 

            yield return null; 
        }
    }
}
