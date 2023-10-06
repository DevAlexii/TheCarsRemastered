using System.Collections;
using UnityEngine;

public class PowerUp_Ramp : PowerUpBase
{
    [SerializeField] private GameObject ramp_prefab;
    private bool isClicked;
    private float bounceTimer;
    [SerializeField] private float MaxbounceTime;
    [SerializeField] private AnimationCurve bounceCurve;
    public override void OnClicked()
    {
        if (!isClicked)
        {
            isClicked = true;
            AudioCallBack.self.PlayAudio(AudioType.Coin, 0.8f);
        }
    }
    public override void Update()
    {
        if (!isClicked)
        {
            base.Update();
        }
        else
        {
            bounceTimer += Time.deltaTime;
            if (bounceTimer >= MaxbounceTime)
            {
                int random_angle = Random.Range(0, 4) * 90;
                Quaternion rot = Quaternion.Euler(Vector3.up * random_angle);
                GameObject ramp = Instantiate(ramp_prefab, Vector3.up * 10f, rot);
                Destroy(this.gameObject);
            }
            float y = bounceCurve.Evaluate(bounceTimer);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }
    }

   
}
