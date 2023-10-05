using UnityEngine;

public class Directional_Arrow_Animation : MonoBehaviour
{
    float max_timer = 1.3f;
    float min_timer = 0.5f;
    float timer;
    float multiply = 2;
    float sinValue = 0;

    void Update()
    {
        if (gameObject.activeSelf)
        {
            timer -= Time.deltaTime * multiply;
            if (timer <= min_timer || timer > max_timer)
            {
                multiply *= -1;
                timer = timer <= min_timer ? min_timer : max_timer;
            }
            sinValue = Mathf.Sin(timer);
            transform.localScale = (Vector3.one * 0.01f) * sinValue * 2;
        }
    }
}
