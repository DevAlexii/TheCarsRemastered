using UnityEngine;

public class ToogleRain : MonoBehaviour
{
    [SerializeField] GameObject RainEffect;
    [SerializeField] float toogle_time;
    float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= toogle_time)
        {
            timer = 0f;
            RainEffect.SetActive(RainEffect.activeSelf ? false : true);
        }
    }
}